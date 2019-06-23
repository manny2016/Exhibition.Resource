


namespace ConsoleApp1
{
    using System.IO;
    using Newtonsoft.Json.Linq;
    using Exhibition.Core;
    using Exhibition.Core.Models;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Host.ConfigureServiceProvider((configure) => { });
            //var array = new byte[] { 12, 35, 33, 23 };
            //var helper = new TaskHelper();
            //helper.Start(new int[] {1,2,3,4,5,6,7,8,9,10 });
            //Console.Read();
            ////Host.ConfigureServiceProvider((configure) => { });
            ////Initial();
            ////var url = "https://mp.weixin.qq.com/s?__biz=MzI3NTUwMzI4Ng==&mid=2247485900&idx=1&sn=b6beb7d5a95ae3a99aabe5f185fe448c&chksm=eb028815dc750103f3169a93ed757b3fa40b87a7892bd98e14e297a58eb88f13dc643d1550d1&mpshare=1&scene=1&srcid=&pass_ticket=hcbje5N2NeFxqkPvC0tcWL02j7VbCzN1%2BJ8x69nPKA6DDfUB5rmDyDwEDwIPuT2I#rd";
            ////var text = url.GetUriContent();
            ///
            Initial();

        }
        static void Initial()
        {

            var service = Host.GetService<IManagementService>();
            var terminals = new List<IBaseTerminal>();
            using (var reader = new StreamReader(Path.Combine(System.Environment.CurrentDirectory, @"BuildinData\Terminal.json")))
            {
                var text = reader.ReadToEnd();
                foreach (var jObject in text.DeserializeToObject<JObject[]>())
                {
                    IBaseTerminal terminal = null;
                    var type = (TerminalTypes)jObject.SelectToken("$.Type").Value<int>();
                    switch (type)
                    {
                        case TerminalTypes.MediaPlayer:
                            terminal = jObject.ToString().DeserializeToObject<MediaPlayerTerminal>();
                            break;
                        case TerminalTypes.SerialPort:
                            terminal = jObject.ToString().DeserializeToObject<SerialPortTerminal>();
                            break;
                    }
                    service.CreateOrUpdate(terminal);
                    terminals.Add(terminal);
                }

            }
            var index = 0;
            using (var reader = new StreamReader(Path.Combine(System.Environment.CurrentDirectory, @"BuildinData\Directives.json")))
            {
                var text = reader.ReadToEnd();
                foreach (var jObject in text.DeserializeToObject<JObject[]>())
                {

                    var name = jObject.SelectToken("$.Name").Value<string>();
                    var defaultWindow = jObject.SelectToken("$.DefaultWindow").ToString();
                    var description = jObject.SelectToken("$.Description").Value<string>();
                    var resources = jObject.SelectToken("$.Resources").ToString();

                    var directive = new Directive()
                    {
                        Name = name,
                        DefaultWindow = defaultWindow.DeserializeToObject<Window>(),
                        Description = description,
                        Resources = resources.DeserializeToObject<Resource[]>(),
                        Terminal = terminals.FirstOrDefault(o => o.Name.Equals(jObject.SelectToken("$.Terminal.name").Value<string>()))
                    };
                    service.CreateOrUpdate(directive);

                }
            }

        }
    }

}
