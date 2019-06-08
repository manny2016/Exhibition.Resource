


namespace ConsoleApp1
{
    using System.IO;
    using Newtonsoft.Json.Linq;
    using Exhibition.Core;
    using Exhibition.Core.Models;
    using System.Linq;
    class Program
    {
        static void Main(string[] args)
        {
            Host.ConfigureServiceProvider((configure) => { });
            var service = Host.GetService<IManagementService>();
            using (var reader = new StreamReader(Path.Combine(System.Environment.CurrentDirectory, @"BuildinData\Terminal.json")))
            {
                var text = reader.ReadToEnd();
                foreach (var jObject in text.DeserializeToObject<JObject[]>())
                {
                    var type = (TerminalTypes)jObject.SelectToken("$.Type").Value<int>();
                    switch (type)
                    {
                        case TerminalTypes.MediaPlayer:
                            service.CreateOrUpdate(jObject.ToString().DeserializeToObject<MediaPlayerTerminal>());
                            break;
                        case TerminalTypes.SerialPort:
                            service.CreateOrUpdate(jObject.ToString().DeserializeToObject<SerialPortTerminal>());
                            break;
                    }
                }

            }
            var index = 0;
            foreach (var directive in service.QueryTerminals(new SQLiteQueryFilter<string>())
                  .Select((ctx) =>
                  {
                      index++;
                      if (ctx.Type == TerminalTypes.MediaPlayer)
                      {
                          return new Directive()
                          {
                              Name = $"D-{index.ToString("00")}",
                              Terminal = ctx,
                              Description = "媒体播放指令",
                              DefaultWindow = (ctx as MediaPlayerTerminal)?.Settings.Windows[0],
                              Resources = new Resource[] {
                                new Resource() {
                                     FullName = "Images/3.jpg",
                                      Name = "3.jpg",
                                       Sorting = 0,
                                        Type = ResourceTypes.Image,
                                         Workspace = "Images"
                                }
                              }
                          };
                      }
                      if (ctx.Type == TerminalTypes.SerialPort)
                      {
                          return new Directive()
                          {
                              Name = $"S-{index.ToString("00")}",
                              Terminal = ctx,
                              Description = "串口指令",
                              Resources = new Resource[] { }
                          };
                      }
                      throw new System.NotSupportedException();
                  }))
            {
                service.CreateOrUpdate(directive);
            }
        }
    }
}
