


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
        private AutoResetEvent Sinal = new AutoResetEvent(true);
        public ListenStates State { get; private set; }
        WaitHandle waitHandle = new AutoResetEvent(true);
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

        public void Run()
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
                            try
                            {
                                Thread.CurrentThread.Join(500);
                                var buffers = new byte[1024];
                                var context = OpenSerialPort(setting);
                                var read = context.SerialPort.Read(buffers, 0, buffers.Length);
                                if (read == 0 || context.Type == ProcessTypes.None) continue;
                                this.WorkflowProcessing(context.Name, buffers.Skip(0).Take(read).ToArray());
                            }
                            catch (TimeoutException timeout)
                            {
                                //Logger.Info($"No bytes were available to read. {setting.PortName};");
                            }
                            catch (Exception ex)
                            {
                                Logger.Error($"issue happended on serialport Listener will re-try after 5 seconds;{ex.SerializeToJson()}");
                            }
                        }
                    });
                    this.State = ListenStates.Stoped;
                });
                scheduler.Start();
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
                            settings.BaudRate,
                            settings.Parity,
                            settings.DataBits);
                        serial.ReadTimeout = 500;
                        serial.WriteTimeout = 500;
                        serial.Handshake = Handshake.XOnXOff;
                        serial.Open();
                        ports[settings.PortName] = new SerialPortWorkContext(settings.PortName, serial);
                        Logger.Info($"Open SerialPort {settings.PortName}");
                    }
                }
                return ports[settings.PortName];
            }
        }

        public void Send(string directiveName, SerialPortSettings settings, byte[] buffers)
        {
            this.Sinal.WaitOne();
            var context = ports[settings.PortName];
            if (context.Useable())
            {

                var type = this.descriptor.DirectiveTypeMapping(directiveName);
                if (type == DirectiveTypes.Move)
                {
                    //Send query directive;            
                    var queryState = "22 A3 A4 A1 0A".Split(' ').Select((ctx) =>
                    {
                        return byte.Parse(ctx, System.Globalization.NumberStyles.HexNumber);
                    }).ToArray();
                    ports[settings.PortName].SerialPort.Write(queryState, 0, queryState.Length);
                    var timeout = DateTime.Now;
                    do
                    {
                        Thread.CurrentThread.Join(500);
                        var states = new byte[1024];
                        var retnVal = ports[settings.PortName].SerialPort.Read(states, 0, states.Length);
                        if (retnVal > 0)
                        {
                            Logger.Info($"Query current postion and return {BitConverter.ToString(states)}");
                            buffers[1] = states[1];
                            ports[settings.PortName].DirectiveName = directiveName;
                            ports[settings.PortName].Type = ProcessTypes.Movefeedback;
                            break;
                        }
                    } while (DateTime.Now.Subtract(timeout).TotalSeconds < 3);
                }
                ports[settings.PortName].SerialPort.Write(buffers, 0, buffers.Length);
            }
            Logger.Info($"Use {settings.PortName} to send bytes {BitConverter.ToString(buffers)}");
            this.Sinal.Set();
        }

        private void WorkflowProcessing(string portName, byte[] data)
        {
            var context = this.ports[portName];
            if (context.Type == ProcessTypes.None) return;
            try
            {

                Logger.Info($"received data from {portName};({data.Length})");
                var hex = BitConverter.ToString(data);
                var matched = this.descriptor.Descriptors.FirstOrDefault((ctx) =>
                {
                    return ctx.Condition.Equals(hex, StringComparison.OrdinalIgnoreCase) &&
                    ctx.Name.Equals(this.ports[portName].Name, StringComparison.OrdinalIgnoreCase);
                });
                if (matched != null)
                {
                    var result = this.descriptor.Endpoint.GetUriJsonContent<GeneralResponse<int>>((http) =>
                    {
                        http.Method = "POST";
                        http.ContentType = "application/json";
                        using (var stream = http.GetRequestStream())
                        {
                            var buffers = System.Text.UTF8Encoding.Default.GetBytes(matched.Context.SerializeToJson());
                            stream.Write(buffers, 0, buffers.Length);
                            stream.Flush();
                        }
                        return http;
                    });
                    Logger.Info($"Matched workflow condition and executed. result:{result.Success}");
                }
                else
                {
                    Logger.Warn($"Can't matche any workflow condition ignore data");
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Issue hanppend on proccess workflow. data:{BitConverter.ToString(data)}");
            }
        }


        private void QueryStateAndReadfeedback(SerialPortSettings settings, Action<SerialPortSettings, byte[]> feedback)
        {
            this.Sinal.WaitOne();
            var context = ports[settings.PortName];
            if (context.Useable())
            {
             
                var queryState = "22 A3 A4 A1 0A".Split(' ').Select((ctx) =>
                {
                    return byte.Parse(ctx, System.Globalization.NumberStyles.HexNumber);
                }).ToArray();
                ports[settings.PortName].SerialPort.Write(queryState, 0, queryState.Length);
                var timeout = DateTime.Now;
                do
                {
                    Thread.CurrentThread.Join(500);
                    var states = new byte[1024];
                    var retnVal = ports[settings.PortName].SerialPort.Read(states, 0, states.Length);
                    if (retnVal > 0)
                    {
                        Logger.Info($"Query current postion and return {BitConverter.ToString(states)}");
                        feedback(settings, states.Skip(0).Take(retnVal).ToArray());
                        break;
                    }
                } while (DateTime.Now.Subtract(timeout).TotalSeconds < 3);
            }
            this.Sinal.Set();
        }

    }
}
