

namespace Exhibition.Core.Services
{
    using Exhibition.Core.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.IO.Ports;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Linq;
    using System.IO;
    public delegate void DataReceivedEventHandler(byte[] bytes, IBaseTerminal terminal);

    public class SerialPortHelper
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(SerialPortHelper));
        private IManagementService service;
        private static object lockObject = new object();
        public ListenStates State { get; private set; }
        private ConcurrentDictionary<string, SerialPort> ports = new ConcurrentDictionary<string, SerialPort>();
        public event DataReceivedEventHandler DataReceived;
        Scheduler scheduler;
        public SerialPortHelper(IManagementService service)
        {
            this.service = service;
            this.State = ListenStates.Stoped;
        }

        public void StartListening()
        {
            if (scheduler == null)
            {
                scheduler = new Scheduler((cancellation) =>
                {
                    var filter = new SQLiteQueryFilter<string>()
                    {
                        TerminalTypes = new TerminalTypes[] { TerminalTypes.SerialPort }
                    };
                    var terminals = this.service.QueryTerminals(filter);
                    Parallel.ForEach(terminals, (terminal) =>
                    {

                        while (cancellation.IsCancellationRequested == false)
                        {
                            try
                            {
                                var serialport = OpenSerialPort((terminal as SerialPortTerminal)?.Settings);
                                this.Send(terminal, new byte[] { });
                                if (serialport == null)
                                {

                                    Thread.CurrentThread.Join(1000 * 5);
                                    Logger.Error($"Cant open serialport on {terminal.Name}; will retry after 5 seconds");
                                    continue;
                                }
                                var buffers = new byte[1024];
                                var retval = serialport.Read(buffers, 0, buffers.Length);
                                if (retval > 0)
                                {
                                    if (DataReceived != null)
                                    {
                                        DataReceived(buffers.Skip(0).Take(retval).ToArray(), terminal);
                                    }
                                    Logger.Info($"received data from {terminal.Name};({retval})");
                                }
                            }
                            catch (TimeoutException timeout)
                            {
                                Logger.Info($"No bytes were available to read. {terminal.Name};");
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

        public void Send(IBaseTerminal terminal, byte[] buffers)
        {
            try
            {
                var settings = (terminal as SerialPortTerminal)?.Settings;
                if (settings == null) throw new NotSupportedException(terminal.Type.ToString());
                var serialport = OpenSerialPort(settings);
                serialport.BaseStream.Write(buffers, 0, buffers.Length);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

    }
}
