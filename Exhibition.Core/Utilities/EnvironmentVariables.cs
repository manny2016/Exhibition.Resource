

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
          
          
        }

        
    }
}
