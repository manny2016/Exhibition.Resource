

namespace Exhibition.Portal.Api.Models
{
    using Exhibition.Core.Models;
    using Newtonsoft.Json;
    public class DirectiveEditModel
    {
        public string Name { get; set; }


        public string Description { get; set; }


        public dynamic Terminal { get; set; }

        [JsonProperty("DefaultWindow", NullValueHandling = NullValueHandling.Ignore)]
        public Window DefaultWindow { get; set; }


        public Resource[] Resources { get; set; }
    }
}
