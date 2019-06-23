


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
                                var context = OpenSerialPort(setting);
                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex.SerializeToJson());
                            }
                            Thread.CurrentThread.Join(1000);
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

        public async void WriteMoveAsync(SerialPortWorkContext context, byte[] buffers)
        {
            var queryState = "22 A3 A4 A1 0A".Split(' ').Select((ctx) =>
            {
                return byte.Parse(ctx, System.Globalization.NumberStyles.HexNumber);
            }).ToArray();
            context.SerialPort.Write(buffers, 0, buffers.Length);
            //Query current postion.
            await this.ReadAsync(context, (ctx, reply) =>
            {
                buffers[1] = reply[1];
                context.SerialPort.Write(buffers, 0, buffers.Length);
                this.ReadAsync(context, (c, b) =>
                {
                    var descriptorObject = this.descriptor.Descriptors.FirstOrDefault(o => o.Context.Name.Equals(context.DirectiveName));
                    if (descriptorObject != null && BitConverter.ToString(b).Equals(descriptorObject.Condition))
                    {
                        this.descriptor.Endpoint.GetUriJsonContent<dynamic>((http) =>
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

                }, 180).GetAwaiter().GetResult();
                this.Sinal.Set();
            });
        }
        public async void WritePowerAnsy(SerialPortWorkContext context, byte[] buffers)
        {
            context.SerialPort.Write(buffers, 0, buffers.Length);
            await this.ReadAsync(context, (ctx, bytes) =>
            {
                this.Sinal.Set();
            });
        }
        public void Send(
            string directiveName,
            SerialPortSettings settings,
            byte[] buffers)
        {
            this.Sinal.WaitOne();
            var context = ports[settings.PortName];
            if (context.Useable())
            {
                var type = this.descriptor.DirectiveTypeMapping(directiveName);
                context.DirectiveName = directiveName;
                switch (type)
                {
                    case DirectiveTypes.Move:
                        this.WriteMoveAsync(context, buffers);
                        break;
                    case DirectiveTypes.SoundOnOff:
                        //this.WritePowerAnsy(context,)
                        this.WritePowerAnsy(context, buffers);
                        break;
                    case DirectiveTypes.MonitorOnOff:
                        if (monitorStates.ContainsKey(directiveName) == false)
                        {
                            monitorStates[directiveName] = buffers[4];
                        }
                        else
                        {
                            if (monitorStates[directiveName] == 0) monitorStates[directiveName] = 1;
                            else monitorStates[directiveName] = 0;
                            buffers[4] = monitorStates[directiveName];
                        }
                        this.WritePowerAnsy(context, buffers);
                        break;
                    case DirectiveTypes.Unknow:
                    default:
                        Logger.Warn($"Unknow directive type ignore it;Directive Name: {directiveName}");
                        break;
                }
            }
            else
            {
                this.Sinal.Set();
            }
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

        /// <summary>
        /// Read Serial Port data by SerialPortSettings
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        /// <param name="timeout">timeout default value is 5 second</param>
        /// <returns></returns>
        private async Task<int> ReadAsync(
            SerialPortWorkContext context,
            Action<SerialPortWorkContext, byte[]> callback = null,
            int timeout = 5)
        {
            var startRead = DateTime.Now;
            int iReadBytes = 0;
            do
            {
                Thread.CurrentThread.Join(1000);
                try
                {
                    var buffers = new byte[64];

                    iReadBytes = context.SerialPort.Read(buffers, 0, buffers.Length);
                    if (iReadBytes > 0)
                    {
                        callback?.Invoke(context, buffers.Skip(0).Take(iReadBytes).ToArray());
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error happened on get reply on {context.Name}. DirectiveName:{context.DirectiveName}");
                }

            } while (DateTime.Now.Subtract(startRead).TotalSeconds < 5 && iReadBytes.Equals(0));
            return iReadBytes;
        }

    }
}
