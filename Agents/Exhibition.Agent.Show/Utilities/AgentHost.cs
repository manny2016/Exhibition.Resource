


namespace Exhibition.Agent.Show
{
    using Exhibition.Core.Services;
    using System;
    using System.ServiceModel;
    using Models = Exhibition.Core.Models;
    using System.Configuration;
    public delegate void OperationEventHandler(object sender, Models::Directive directive);
    public static class AgentHost
    {
        public static event OperationEventHandler DirectiveReceived;
        public static void HostOperationSerivceViaConfiguration()
        {
            var host = new ServiceHost(typeof(OperationService));
            host.Opened += delegate
            {
                Console.WriteLine("Operation Service has begun to listen ... ...");
            };
            host.Open();
        }
        public static void TriggerDirectiveEvent(object sender, Models::Directive directive)
        {
            DirectiveReceived?.Invoke(sender, directive);
        }

        public static string Api
        {
            get
            {
                var url = ConfigurationManager.AppSettings["api"];
                if (string.IsNullOrEmpty(url))
                    url = "https://localhost:44378/";
                return url;
            }
        }
        public static string TerminalName
        {
            get
            {
                var name = ConfigurationManager.AppSettings["name"];
                return name;
            }
        }
        public static string Resource {
            get
            {
                var text= ConfigurationManager.AppSettings["resource"];
                return text;
            }
        }
    }
}
