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
    }
    public class Descriptor
    {
        public string Condition { get; set; }
        public OperationContextforApi Context { get; set; }
    }
}
