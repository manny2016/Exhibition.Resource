using Exhibition.Core;
using Exhibition.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhibition.Agent.Show.Models
{
    public class OperationContext : ICloneable
    {
        public MediaControlDirective Directive { get; set; }

        public DirectiveTypes Type { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public T Clone<T>()
            where T : class
        {
            return this.MemberwiseClone() as T;
        }
        public OperationContext DeepClone()
        {
            return this.SerializeToJson().DeserializeToObject<OperationContext>();
        }
    }
}
