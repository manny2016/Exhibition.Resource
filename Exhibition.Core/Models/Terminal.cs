

namespace Exhibition.Core.Models
{
    public class Terminal
    {
        [Newtonsoft.Json.JsonProperty("ip")]
        public string Ip { get; set; }

        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }

        [Newtonsoft.Json.JsonProperty("schematic")]
        public string Schematic { get; set; }

        [Newtonsoft.Json.JsonProperty("endpoint")]
        public string Endpoint { get; set; }

        [Newtonsoft.Json.JsonProperty("windows")]
        public Window[] Windows { get; set; }
    }
}
