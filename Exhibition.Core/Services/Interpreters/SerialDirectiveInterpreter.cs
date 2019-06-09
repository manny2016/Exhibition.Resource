

namespace Exhibition.Core.Services
{
    using System;
    using Models = Exhibition.Core.Models;
    using System.Text;
    public class SerialDirectiveInterpreter : DirectiveInterpreter
    {
        public SerialDirectiveInterpreter(IOperateContext directive)
            : base(directive)
        {

        }
        public override void Execute()
        {
            var helper = Host.GetService<SerialPortHelper>();
            helper.Send(this.Context.Directive.Terminal, UTF8Encoding.ASCII.GetBytes("abcdefg"));

        }
    }
}
