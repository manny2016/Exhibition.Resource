

namespace Exhibition.Core.Models
{
    public class OperationContext : IOperateContext
    {
        public Directive Directive { get; set; }
        public DirectiveTypes Type { get; set; }
    }
}
