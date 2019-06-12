using Microsoft.Extensions.DependencyInjection;
using SerialPortHelper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortHelper
{
    public class SerialPortHelperConfiguration
    {
        public static void HostOperationSerivceViaConfiguration()
        {
            var host = new ServiceHost(typeof(OperationService));
            host.Opened += delegate
            {
                Console.WriteLine("Operation Service has begun to listen ... ...");
            };
            host.Open();
        }

     
    }
}
