

namespace Exhibition.Core
{
    using System.IO;
    using System.Collections.Generic;
    using Exhibition.Core.Services;
    using Exhibition.Core.Models;
    using Newtonsoft.Json.Linq;
    using System.Linq;

    public static class EnvironmentVariables
    {
        public const string ROOT = @"assets\userfiles";
        public const string UrlROOT = @"assets/userfiles";

        public static readonly string[] SupportImages = new string[] {
            ".jpg",".png",".jpeg",".bmp"
        };
        public static readonly string[] SupportVideos = new string[] {
            ".mp4",".avi",".mpeg"
        };
        public static readonly string[] SupportTxtPlain = new string[] {
            ".txt"
        };
        //public static readonly string[] SupportSerialPortDirective = new string[] {
        //    ".serial"
        //};

        private static readonly Dictionary<string, TerminalTypes> terminalTypeMappings = new Dictionary<string, TerminalTypes>()
        {
            { "音响",TerminalTypes.SerialPort},
            { "滑屏",TerminalTypes.SerialPort},
            { "液晶屏",TerminalTypes.SerialPort},
            { "媒体播放机",TerminalTypes.MediaPlayer}
        };

        public static void InitializeTestData()
        {            
            var service = new ManagementService();
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
