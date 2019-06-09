using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exhibition.Core.Entities
{
    public class Directive
    {


        public virtual string Name { get; set; }


        public virtual string Description { get; set; }

        public virtual string TargetName { get; set; }

        public virtual string Target { get; set; }


        public virtual string DefaultWindow { get; set; }


        public virtual string Resources { get; set; }
    }
}
