using Exhibition.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhibition.Agent.Show
{
    public delegate void OperationEventHandler(object sender, OperationEventArgs e);
    public class OperationEventArgs
    {
        public IOperateContext Context { get; set; }
    }
}
