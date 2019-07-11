using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortHelper.Models
{
    public class WorkflowDescriptor
    {
        public string Endpoint { get; set; }

        public Descriptor[] Descriptors { get; set; }

        public string[] DirectiveforMove { get; set; }
        public string[] DirectiveforPower { get; set; }
        public string[] MoveSerialPorts { get; set; }
        public string[] SoundSerialPorts { get; set; }
    }
    public class Descriptor
    {
        public string Condition { get; set; }
        public string Name { get; set; }
        public OperationContextforApi Context { get; set; }
    }
}
