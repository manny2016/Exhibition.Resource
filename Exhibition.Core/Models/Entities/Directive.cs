using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exhibition.Core.Entities
{
    public class Directive
    {

        public virtual DirectiveTypes Type { get; set; }

        public virtual string Name { get; set; }

        public virtual string TargetIp { get; set; }

        public virtual int? Window { get; set; }

        public virtual string Description { get; set; }

        public virtual string Context { get; set; }
    }
}
