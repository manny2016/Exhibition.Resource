

namespace Exhibition.Core.Services
{
    using System;
    using Models = Exhibition.Core.Models;
    public abstract class DirectiveInterpreter
    {
        protected DirectiveInterpreter(IOperateContext context)
        {
            this.Context = context;
        }

        public static DirectiveInterpreter Create(IOperateContext context)
        {
            switch (context.Directive.Terminal.Type)
            {
                case TerminalTypes.MediaPlayer:
                    return new MediaDirectiveInterpreter(context);

                case TerminalTypes.SerialPort:
                    return new SerialDirectiveInterpreter(context);
            }
            throw new NotSupportedException(context.Directive.Terminal.Name);
        }
        protected IOperateContext Context
        {
            get;
            set;
        }
        public abstract void Execute();
    }


}
