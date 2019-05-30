


namespace Exhibition.Core.Models
{

    public class Resource
    {
      

        [Newtonsoft.Json.JsonProperty("name")]
        public string Name { get; set; }

        [Newtonsoft.Json.JsonProperty("type")]
        public ResourceTypes Type { get; set; }

        [Newtonsoft.Json.JsonProperty("workspace")]
        public string Workspace { get; set; }

        [Newtonsoft.Json.JsonProperty("fullName")]
        public string FullName { get; set; }

        [Newtonsoft.Json.JsonProperty("sorting")]
        public int Sorting { get; set; }
    }
}
