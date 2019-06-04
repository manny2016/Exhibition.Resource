

namespace Exhibition.Core.Models
{
    public class MedaiPlayerSettings
    {
        [Newtonsoft.Json.JsonProperty("ip")]
        public string Ip { get; set; }

        [Newtonsoft.Json.JsonProperty("endpoint")]
        public string Endpoint { get; set; }

        [Newtonsoft.Json.JsonProperty("windows")]
        public Window[] Windows { get; set; }
    }
    
}
