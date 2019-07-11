


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
            foreach (var terminal in terminals)
            {
                foreach(var directive in CreateDirective(terminal))
                {
                    service.CreateOrUpdate(directive);
                }
            }
        }
        static IEnumerable<Directive> CreateDirective(IBaseTerminal terminal)
        {
            switch (terminal.Name)
            {
                case "PC-01":
                    foreach (var directive in CreateDirectivePC01(terminal))
                        yield return directive;
                    break;
                case "PC-02":
                    foreach (var directive in CreateDirectivePC02(terminal))
                        yield return directive;
                    break;
                case "PC-03":
                    foreach (var directive in CreateDirectivePC03(terminal))
                        yield return directive;
                    break;
                case "PC-04":
                    foreach (var directive in CreateDirectivePC04(terminal))
                        yield return directive;
                    break;
                case "PC-05":
                    foreach (var directive in CreateDirectivePC05(terminal))
                        yield return directive;
                    break;
                case "PLC-01":
                    foreach (var directive in CreateDirectivePLC01(terminal))
                        yield return directive;
                    break;
                case "PLC-02":
                    foreach (var directive in CreateDirectivePLC02(terminal))
                        yield return directive;
                    break;
            }

        }
        static IEnumerable<Directive> CreateDirectivePC01(IBaseTerminal terminal)
        {
            yield return new Directive()
            {
                Name = "D-01",
                DefaultWindow = (terminal as MediaPlayerTerminal)?.Settings.Windows[0],
                Terminal = terminal,
                Description = "张集发布",
                Resources = new Resource[] { }
            };
            yield return new Directive()
            {
                Name = "D-07",
                DefaultWindow = (terminal as MediaPlayerTerminal)?.Settings.Windows[0],
                Description = "铜山视频",
                Resources = new Resource[] { },
                Terminal = terminal
            };
        }
        static IEnumerable<Directive> CreateDirectivePC02(IBaseTerminal terminal)
        {
            yield return new Directive()
            {
                Name = "D-02",
                DefaultWindow = (terminal as MediaPlayerTerminal)?.Settings.Windows[0],
                Description = "志愿者服务",
                Resources = new Resource[] { },
                Terminal = terminal
            };
        }
        static IEnumerable<Directive> CreateDirectivePC03(IBaseTerminal terminal)
        {
            yield return new Directive()
            {
                Name = "D-03",
                DefaultWindow = (terminal as MediaPlayerTerminal)?.Settings.Windows[0],
                Description = "实践阵地(滑屏媒体)",
                Resources = new Resource[] { },
                Terminal = terminal
            };
        }
        static IEnumerable<Directive> CreateDirectivePC04(IBaseTerminal terminal)
        {
            yield return new Directive()
            {
                Name = "D-08",
                DefaultWindow = (terminal as MediaPlayerTerminal)?.Settings.Windows[0],
                Description = "领军人物-01",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "D-09",
                DefaultWindow = (terminal as MediaPlayerTerminal)?.Settings.Windows[1],
                Description = "领军人物-02",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "D-10",
                DefaultWindow = (terminal as MediaPlayerTerminal)?.Settings.Windows[2],
                Description = "创新产品-01",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "D-11",
                DefaultWindow = (terminal as MediaPlayerTerminal)?.Settings.Windows[3],
                Description = "创新产品-02",
                Resources = new Resource[] { },
                Terminal = terminal
            };
        }
        static IEnumerable<Directive> CreateDirectivePC05(IBaseTerminal terminal)
        {
            yield return new Directive()
            {
                Name = "D-12",
                DefaultWindow = (terminal as MediaPlayerTerminal)?.Settings.Windows[0],
                Description = "文明之花",
                Resources = new Resource[] { },
                Terminal = terminal
            };
        }
        static IEnumerable<Directive> CreateDirectivePLC01(IBaseTerminal terminal)
        {
            yield return new Directive()
            {
                Name = "S-15",                
                Description = "滑屏 位置 0",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-16",
                Description = "滑屏 位置 1",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-17",
                Description = "滑屏 位置 2",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-18",
                Description = "滑屏 位置 3",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-19",
                Description = "滑屏 位置 4",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-20",
                Description = "滑屏 位置 5",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-21",
                Description = "滑屏 位置 6",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-22",
                Description = "滑屏 位置 7",
                Resources = new Resource[] { },
                Terminal = terminal
            };
        }
        static IEnumerable<Directive> CreateDirectivePLC02(IBaseTerminal terminal)
        {
            yield return new Directive()
            {
                Name = "S-08",
                Description = "第一展区讲解系统控制",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-09",
                Description = "第二展区讲解系统控制",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-10",
                Description = "第三展区讲解系统控制",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-11",
                Description = "第四展区讲解系统控制",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-12",
                Description = "第五展区讲解系统控制",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-13",
                Description = "第六展区讲解系统控制-01",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            yield return new Directive()
            {
                Name = "S-14",
                Description = "第六展区讲解系统控制-02",
                Resources = new Resource[] { },
                Terminal = terminal
            };
            
        }
    }

}
