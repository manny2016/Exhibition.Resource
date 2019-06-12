


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

    public delegate void DataReceivedEventHandler(byte[] bytes, SerialPortSettings setting);
    public class SerialPortService
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(SerialPortService));
        private readonly IEnumerable<SerialPortSettings> settings;
        private static object lockObject = new object();
        private Scheduler scheduler;
        private ConcurrentDictionary<string, SerialPort> ports = new ConcurrentDictionary<string, SerialPort>();
        public ListenStates State { get; private set; }
        public event DataReceivedEventHandler DataReceived;
        public SerialPortService(IEnumerable<SerialPortSettings> settings)
        {
            this.settings = settings.GroupBy(o => o.PortName).Select((ctx) =>
            {
                return ctx.FirstOrDefault();
            });
        }

        public void Run()
        {
            if (scheduler == null)
            {
                scheduler = new Scheduler((cancellation) =>
                {
                    Parallel.ForEach(this.settings, (setting) =>
                    {

                        while (cancellation.IsCancellationRequested == false)
                        {
                            try
                            {
                                var serialport = OpenSerialPort(setting);
                                var buffers = new byte[1024];
                                var retval = serialport.Read(buffers, 0, buffers.Length);
                                if (retval > 0)
                                {
                                    if (DataReceived != null)
                                    {
                                        DataReceived(buffers.Skip(0).Take(retval).ToArray(), setting);
                                    }
                                    Logger.Info($"received data from {setting.PortName};({retval})");
                                }
                            }
                            catch (TimeoutException timeout)
                            {
                               // Logger.Info($"No bytes were available to read. {setting.PortName};");
                            }
                            catch (Exception ex)
                            {
                                Logger.Error($"issue happended on serialport Listener will re-try after 5 seconds;{ex.SerializeToJson()}");
                                Thread.CurrentThread.Join(1000 * 5);
                            }
                            Thread.CurrentThread.Join(500);
                        }
                    });
                });
                scheduler.Start();
                this.State = ListenStates.Listening;
            }
        }
        public void Abort()
        {
            if (scheduler != null)
            {
                scheduler.Abort();
                foreach (var serialPort in ports)
                {
                    if (serialPort.Value.IsOpen)
                        serialPort.Value.Close();
                }
                this.State = ListenStates.Stoped;
            }
        }

        private SerialPort OpenSerialPort(SerialPortSettings settings)
        {
            if (settings == null) return null;
            lock (lockObject)
            {
                if (!ports.ContainsKey(settings.PortName) || ports[settings.PortName].IsOpen == false)
                {
                    lock (lockObject)
                    {

                        ports[settings.PortName] = new SerialPort(settings.PortName,
                            settings.BaudRate,
                            settings.Parity,
                            settings.DataBits);
                        ports[settings.PortName].ReadTimeout = 500;
                        ports[settings.PortName].WriteTimeout = 500;
                        ports[settings.PortName].Handshake = Handshake.XOnXOff;
                        ports[settings.PortName].Open();
                        Logger.Info($"Open SerialPort {settings.PortName}");
                    }
                }
                return ports[settings.PortName];
            }
        }
    }
}
