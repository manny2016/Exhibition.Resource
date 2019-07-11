


namespace Exhibition.Agent.Show
{
    using Exhibition.Core.Services;
    using System;
    using System.ServiceModel;
    using Models = Exhibition.Core.Models;
    using System.Configuration;
    
    public static class AgentHost
    {
        public static event OperationEventHandler DirectiveReceived;
        public static event LayoutInfoEventHandler ShowLayoutInfo;
        public static event LayoutInfoEventHandler UpgradeLayoutInfo;
        public static void HostOperationSerivceViaConfiguration()
        {
            var host = new ServiceHost(typeof(OperationService));
            host.Opened += delegate
            {
                Console.WriteLine("Operation Service has begun to listen ... ...");
            };
            host.Open();
        }
        public static void TriggerDirectiveEvent(object sender, OperationEventArgs e)
        {
            DirectiveReceived?.Invoke(sender, e);
        }
        public static void TriggerShowLayoutInfoEvent(object sender,LayoutinfoEventArgs e)
        {
            ShowLayoutInfo?.Invoke(sender, e);
        }
        public static void TriggerUpgardLayout(object sender, LayoutinfoEventArgs e)
        {
            UpgradeLayoutInfo?.Invoke(sender, e);
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
