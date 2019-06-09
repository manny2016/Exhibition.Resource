using Exhibition.Core;
using Exhibition.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhibition.Agent.Show.Models
{
    public class OperationContext
    {
        public MediaControlDirective Directive { get; set; }

        public DirectiveTypes Type { get; set; }
    }
}
