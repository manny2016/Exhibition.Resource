using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortHelper.Models
{
    public class OperationContext
    {
        public string Content { get; set; }
        public SerialPortSettings Settings { get; set; }
    }
}
