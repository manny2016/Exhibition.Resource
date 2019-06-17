using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortHelper.Models
{
    public class SerialPortWorkContext
    {
        public SerialPortWorkContext(string name,
            SerialPort serialPort,
            string directiveName = null,
            ProcessTypes type = ProcessTypes.None)
        {
            this.Name = name;
            this.DirectiveName = directiveName;
            this.SerialPort = serialPort;
            this.Type = type;
        }
        public string Name { get; private set; }
        public string DirectiveName { get; set; }
        public ProcessTypes Type { get; set; }
        public SerialPort SerialPort { get; private set; }

        public bool Useable()
        {
            return this.SerialPort != null && this.SerialPort.IsOpen == true;
        }
    }
}
