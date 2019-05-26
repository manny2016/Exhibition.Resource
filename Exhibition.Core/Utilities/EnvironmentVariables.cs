

namespace Exhibition.Core
{
    using System.IO;
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
    }
}
