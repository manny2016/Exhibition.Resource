namespace Exhibition.Core
{
    using System;
    using System.Net;
    using System.Threading;

    public class DefaultTransientFaultHandler<T> : ITransientFaultHandler<T, Exception>
    {
        
        public ITransientFaultDetecter<Exception> Detecter { get; protected set; }
        public Func<T> Function { get; protected set; }
        public int RetryThreshold { get; protected set; }

        public DefaultTransientFaultHandler(
            Func<T> func,
            ITransientFaultDetecter<Exception> detecter,
            int retryThreshold)
        {
            this.Function = func;
            this.Detecter = detecter;
            this.RetryThreshold = retryThreshold;
        }

        public T Execute()
        {
            if (this.Function != null)
            {
                int tryCount = 0;
                while (true)
                {
                    tryCount++;
                    try
                    {
                        return this.Function();
                    }
                    catch (Exception ex)
                    {
                        if (tryCount == 1)
                        {
                            //Logger.Error("Transient Fault First Retrying (default)", ex);
                            //Logger.LogException(
                            //    "Transient Fault First Retrying (default)",
                            //    ex);
                        }
                        if ((this.Detecter.Detect(ex, true) == false)
                            || (tryCount >= this.RetryThreshold))
                        {
                            throw ex;
                        }
                    }
                }
            }
            return default(T);
        }

        public static T ExecuteWithDefaultHttpDetecter(Func<T> func)
        {
            return Execute(
                func,
                new DefaultHttpTransientFaultDetecter(),
                DefaultHttpTransientFaultDetecter.DEFAULT_RETRY_THRESHOLD);
        }

        public static T Execute(
            Func<T> func,
            ITransientFaultDetecter<Exception> detecter,
            int retryThreshold)
        {
            var handler = new DefaultTransientFaultHandler<T>(
                func,
                detecter,
                retryThreshold);
            return handler.Execute();
        }
    }
}