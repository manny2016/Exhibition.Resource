using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Exhibition.Core.Models
{
    public class Window
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("location")]
        public WinPoint Location { get; set; }

        [Newtonsoft.Json.JsonProperty("size")]
        public WinSize Size { get; set; }

        [Newtonsoft.Json.JsonProperty("monitor")]
        public int Monitor { get; set; }
    }

    public class WinSize
    {
        [Newtonsoft.Json.JsonProperty("width")]
        public int Width { get; set; }
        [Newtonsoft.Json.JsonProperty("height")]
        public int Height { get; set; }
    }
    public class WinPoint {
        [Newtonsoft.Json.JsonProperty("x")]
        public int X { get; set; }
        [Newtonsoft.Json.JsonProperty("y")]
        public int Y { get; set; }
    }
}
