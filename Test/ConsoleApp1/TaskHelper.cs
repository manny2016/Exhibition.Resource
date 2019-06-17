using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class TaskHelper
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(TaskHelper));


        protected AutoResetEvent Sinal = new AutoResetEvent(true);
        public virtual void Start(int[] array)
        {
            Parallel.ForEach(array, (item) =>
            {
               
                while (true)
                {
                    this.Sinal.WaitOne();
                    DateTime dt = DateTime.Now;
                    Thread.CurrentThread.Join(1000);
                    Logger.Info($"i:{item}, {DateTime.Now.ToString("HH:mm:ss")} time span;{DateTime.Now.Subtract(dt).TotalSeconds} sec.");
                    this.Sinal.Set();
                }
            });

        }

    }
}
