

namespace Exhibition.Core.Models
{
    using Newtonsoft.Json;
    public class Directive
    {

        
        public string Name { get; set; }

        
        public string Description { get; set; }

        
        public IBaseTerminal Terminal { get; set; }

        [JsonProperty("DefaultWindow",NullValueHandling = NullValueHandling.Ignore)]
        public Window DefaultWindow { get; set; }

        
        public Resource[] Resources { get; set; }
    }

}
