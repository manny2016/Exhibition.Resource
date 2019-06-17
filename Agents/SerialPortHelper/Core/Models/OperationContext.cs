using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortHelper.Models
{
    public class OperationContext
    {
        public string Name { get; set; }
        public string[] HexString { get; set; }
        public SerialPortSettings Settings { get; set; }
      
    }
    public class OperationContextforApi
    {
        public string Name { get; set; }
        public int Type { get; set; }
    }
}
