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
        public Point Location { get; set; }

        [Newtonsoft.Json.JsonProperty("size")]
        public Size Size { get; set; }

        [Newtonsoft.Json.JsonProperty("monitor")]
        public int Monitor { get; set; }
    }
}
