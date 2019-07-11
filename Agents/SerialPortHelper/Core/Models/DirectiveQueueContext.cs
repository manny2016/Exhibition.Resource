using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortHelper.Models
{
    public class DirectiveQueueContext
    {
      
        public string Name { get; set; }
        public string PortName { get; set; }
        public byte[] Buffers { get; set; }
    }
}
