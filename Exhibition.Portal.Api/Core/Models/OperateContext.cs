using Exhibition.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exhibition.Portal.Api.Models
{
    public class OperateContext
    {
        public string Name { get; set; }

        public DirectiveTypes Type { get; set; }
    }
}
