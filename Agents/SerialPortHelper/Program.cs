



namespace SerialPortHelper
{
    using Newtonsoft.Json.Linq;
    using SerialPortHelper.Services;
    using System;
    using System.IO;
    using System.Text;
    using System.Linq;
    using SerialPortHelper.Models;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            var workflow = new WorkflowDescriptor()
            {
                 Endpoint = "http://localhost:8080/api/mgr/Execute"
            };
            SerialPortHelperConfiguration.HostOperationSerivceViaConfiguration();
            AgentHost.Configure((collection) =>
            {
            
            });
            AgentHost.GetService<SerialPortService>()
                .Run();
        }


    }
}
