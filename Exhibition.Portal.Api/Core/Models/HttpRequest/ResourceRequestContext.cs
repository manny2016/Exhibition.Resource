

namespace Exhibition.Portal.Api.Models
{
    public class ResourceRequestContext
    {
        [Newtonsoft.Json.JsonProperty("workspace")]
        public string Workspace { get; set; }

        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("newName")]
        public string NewName { get; set; }
    }
}
