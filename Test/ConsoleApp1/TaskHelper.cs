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

        
        public async Task StartAsync(int[] array)
        {
            Parallel.ForEach(array, (item) => {
                for (var j = 0; j < 100000; j++)
                {
                    Logger.Info($"i:{item};j:{j}");
                    Thread.CurrentThread.Join(1000);
                }
            });
          
        }
        
    }
}
