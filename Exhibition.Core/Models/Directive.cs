

namespace Exhibition.Core.Models
{
    public class Directive
    {

        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }

        [Newtonsoft.Json.JsonProperty("type")]
        public DirectiveTypes Type { get; set; }

        [Newtonsoft.Json.JsonProperty("terminal")]
        public IBaseTerminal Terminal { get; set; }

        [Newtonsoft.Json.JsonProperty("defaultWindow")]
        public Window DefaultWindow { get; set; }

        [Newtonsoft.Json.JsonProperty("resources")]
        public Resource[] Resources { get; set; }
    }

}
