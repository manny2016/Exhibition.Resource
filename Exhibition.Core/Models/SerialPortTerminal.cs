using System;
using System.Collections.Generic;
using System.Text;

namespace Exhibition.Core
{
    public class SerialPortTerminal : ITerminal<SerialPortSettings>
    {
        public SerialPortSettings Settings { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TerminalTypes Type { get { return TerminalTypes.SerialPort; } }

        public string GetSettings()
        {
            return this.Settings.SerializeToJson();
        }
    }
}
