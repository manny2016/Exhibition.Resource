


namespace SerialPortHelper.Services
{
    using SerialPortHelper.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Threading;
    using System.IO.Ports;
    using System.Collections.Concurrent;
    using System;

    //public delegate void DataReceivedEventHandler(byte[] bytes, SerialPortSettings setting);
    public class SerialPortService
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(SerialPortService));
        private readonly IEnumerable<SerialPortSettings> settings;
        private static object lockObject = new object();
        private readonly WorkflowDescriptor descriptor;
        private Scheduler scheduler;
        private ConcurrentDictionary<string, SerialPortWorkContext> ports = new ConcurrentDictionary<string, SerialPortWorkContext>();
        private ConcurrentDictionary<string, byte> monitorStates = new ConcurrentDictionary<string, byte>();
        private ConcurrentDictionary<string, ConcurrentQueue<DirectiveQueueContext>> directives = new ConcurrentDictionary<string, ConcurrentQueue<DirectiveQueueContext>>();
        //private ConcurrentQueue<byte[]> moves = new ConcurrentQueue<byte[]>();
        private AutoResetEvent Sinal = new AutoResetEvent(true);
        public ListenStates State { get; private set; }
        WaitHandle waitHandle = new AutoResetEvent(true);
        private readonly byte[] queries = "22 00 00 00 0A".Split(' ').Select((ctx) =>
        {
            return byte.Parse(ctx, System.Globalization.NumberStyles.HexNumber);
        }).ToArray();
        private readonly byte[] allSoundOff = "11 A8 B0 00 0A".Split(' ').Select((ctx) =>
        {
            return byte.Parse(ctx, System.Globalization.NumberStyles.HexNumber);

        }).ToArray();
        public SerialPortService(
            IEnumerable<SerialPortSettings> settings,
            WorkflowDescriptor descriptor)
        {
            this.settings = settings.GroupBy(o => o.PortName).Select((ctx) =>
            {
                return ctx.FirstOrDefault();
            });
            this.descriptor = descriptor;
        }

        public async void Run()
        {
            if (scheduler == null)
            {
                scheduler = new Scheduler((cancellation) =>
                {
                    this.State = ListenStates.Listening;
                    Parallel.ForEach(this.settings, (setting) =>
                    {
                        while (cancellation.IsCancellationRequested == false)
                        {
                            var context = OpenSerialPort(setting);
                            this.DoWork(context);
                        }
                    });
                    this.State = ListenStates.Stoped;
                });
                scheduler.Start();
            }
        }
        private void Read(SerialPortWorkContext context, byte[] buffers, Action<byte[]> action)
        {
            try
            {              
                context.SerialPort.Write(buffers, 0, buffers.Length);
                Thread.CurrentThread.Join(500);
                var reply = new byte[64];
                var read = context.SerialPort.Read(reply, 0, reply.Length);
                if (read > 0)
                {
                    action?.Invoke(reply.Skip(0).Take(read).ToArray());
                }
                else
                {
                    action?.Invoke(null);
                }
            }
            catch (TimeoutException ex)
            {
                Logger.Warn($"Read time out on {context.SerialPort.PortName}");
            }
            catch (Exception ex)
            {
                Logger.Error($"UnKnow exception {ex.SerializeToJson()}");
            }

        }
        public void Abort()
        {
            if (scheduler != null)
            {
                scheduler.Abort();
                foreach (var serialPort in ports)
                {
                    if (serialPort.Value.SerialPort.IsOpen)
                        serialPort.Value.SerialPort?.Close();
                }
                this.State = ListenStates.Stoped;
            }
        }

        private SerialPortWorkContext OpenSerialPort(SerialPortSettings settings)
        {
            if (settings == null) return null;
            lock (lockObject)
            {
                if (!ports.ContainsKey(settings.PortName) || ports[settings.PortName].SerialPort.IsOpen == false)
                {
                    lock (lockObject)
                    {
                        var serial = new SerialPort(settings.PortName,
                          9600, Parity.None,
                            8, StopBits.One);

                        serial.Open();
                        ports[settings.PortName] = new SerialPortWorkContext(settings.PortName, serial);
                        Logger.Info($"Open SerialPort {settings.PortName}");
                    }
                }
                return ports[settings.PortName];
            }
        }

        private void DoWork(SerialPortWorkContext context)
        {
            try
            {
                this.Sinal.WaitOne();                
                if (this.descriptor.MoveSerialPorts.Any(o => o.Equals(context.SerialPort.PortName)))
                {
                    this.Read(context, queries, (replies) =>
                     {
                         Logger.Warn($"Current position {BitConverter.ToString(replies)}");
                         if (replies != null && this.directives[context.Name].TryDequeue(out DirectiveQueueContext directive))
                         {                             
                             directive.Buffers[1] = replies[1];
                             context.SerialPort.Write(directive.Buffers, 0, directive.Buffers.Length);
                             Logger.Info($"Send move directive {BitConverter.ToString(directive.Buffers)}");
                             this.DoWorkflow(directive.Name);
                         }
                     });
                }
                else
                {
                    if (this.directives[context.Name].TryDequeue(out DirectiveQueueContext directive))
                    {
                        this.Read(context, allSoundOff, (replies) =>
                        {
                            context.SerialPort.Write(directive.Buffers, 0, directive.Buffers.Length);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                this.Sinal.Set();
            }
        }

        public void Send(
            string directiveName,
            SerialPortSettings settings,
            byte[] buffers)
        {
            var context = new DirectiveQueueContext()
            {
                PortName = settings.PortName,
                Buffers = buffers,
                Name = directiveName
            };
            if (!directives.ContainsKey(settings.PortName))
            {
                directives[settings.PortName] = new ConcurrentQueue<DirectiveQueueContext>();
                directives[settings.PortName].Enqueue(context);
            }
            else
            {
                directives[settings.PortName].Enqueue(context);
            }
        }
        private void DoWorkflow(string directiveName)
        {
            foreach (var descriptorObject in this.descriptor.Descriptors
                .Where(o => o.Condition.Equals(directiveName, StringComparison.OrdinalIgnoreCase)))
            {
                this.descriptor.Endpoint.GetUriJsonContent<GeneralResponse<int>>((http) =>
                {
                    http.Method = "POST";
                    http.ContentType = "application/json";
                    var data = descriptorObject.Context.SerializeToJson();
                    using (var stream = http.GetRequestStream())
                    {
                        var bytes = System.Text.UTF8Encoding.Default.GetBytes(data);
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }
                    return http;
                });
            }
        }
    }
}
