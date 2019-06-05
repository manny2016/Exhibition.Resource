

namespace Exhibition.Core
{
    using System.IO;
    using System.Collections.Generic;
    using Exhibition.Core.Services;
    using Exhibition.Core.Models;

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
        public static readonly string[] SupportWebPages = new string[] {
            ".link"
        };
        public static readonly string[] SupportSerialPortDirective = new string[] {
            ".serial"
        };

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
            service.CreateOrUpdate(new MediaPlayerTerminal()
            {
                Description = "视频播放器",
                Name = "01",
                Settings = new MedaiPlayerSettings()
                {
                    Endpoint = "http://192.168.0.102/api/common/run",
                    Ip = "192.168.0.102",
                    Windows = new Window[] {
                         new Window(){  Id =1, Location = new WinPoint(){ X=0,Y=0 } , Size= new WinSize(){  Width=1024, Height = 768} },
                         new Window(){  Id =2, Location = new WinPoint(){ X=1024,Y=0 } , Size= new WinSize(){  Width=1024, Height = 768} },
                         new Window(){  Id =2, Location = new WinPoint(){ X=2048,Y=0 } , Size= new WinSize(){  Width=1024, Height = 768} },
                    }
                }
            });

            service.CreateOrUpdate(new SerialPortTerminal()
            {
                Description = "PLC中控主机",
                Name = "PLC",
                Settings = new SerialPortSettings()
                {
                    BaudRate = 9600,
                    DataBits = 8,
                    Parity = System.IO.Ports.Parity.Even,
                    PortName = "COM1",
                    StopBits = System.IO.Ports.StopBits.One
                }
            });

            service.CreateOrUpdate(new Directive()
            {
                DefaultWindow = new Window() { Id = 1, Location = new WinPoint() { X = 0, Y = 0 }, Size = new WinSize() { Width = 1024, Height = 768 } },
                Description = "播放视频",
                Name = "播放宣传片",
                Resources = new Resource[] { new Resource() { Name = "1.avi", Workspace = "videos", Type = ResourceTypes.Video } },
                Type = DirectiveTypes.Run,
                Terminal = new MediaPlayerTerminal()
                {
                    Description = "视频播放器",
                    Name = "大屏",
                    Settings = new MedaiPlayerSettings()
                    {
                        Endpoint = "http://192.168.0.102/api/common/run",
                        Ip = "192.168.0.102",
                        Windows = new Window[] {
                         new Window(){  Id =1, Location = new WinPoint(){ X=0,Y=0 } , Size= new WinSize(){  Width=1024, Height = 768} },
                         new Window(){  Id =2, Location = new WinPoint(){ X=1024,Y=0 } , Size= new WinSize(){  Width=1024, Height = 768} },
                         new Window(){  Id =2, Location = new WinPoint(){ X=2048,Y=0 } , Size= new WinSize(){  Width=1024, Height = 768} },
                    }
                    }
                }

            });
        }
    }
}
