namespace Exhibition.Core
{
    using System;

    public class ExceptionTransientFaultDetecter
        : ITransientFaultDetecter<Exception>
    {
        protected Func<Exception, bool, bool> DetectFunction;
        public ExceptionTransientFaultDetecter(
            Func<Exception, bool, bool> detect)
        {
            this.DetectFunction = detect;
        }

        public virtual bool Detect(Exception condition,bool ifHasDetailErrorMessageThrowIt = false)
        {
            if (this.DetectFunction == null)
            {
                return true;
            }
            else
            {
                return this.DetectFunction(condition, ifHasDetailErrorMessageThrowIt);
            }
        }
    }
}