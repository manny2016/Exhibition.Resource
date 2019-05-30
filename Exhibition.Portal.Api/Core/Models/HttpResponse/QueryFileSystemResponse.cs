

using Exhibition.Core;
using Exhibition.Core.Models;

namespace Exhibition.Portal.Api.Models
{
    public class QueryFileSystemResponse : Response<Resource[]>
    {

        public QueryFileSystemResponse(string current) :
            base()
        {
            this.Workspace = current;
        }
        public QueryFileSystemResponse() :
            this(EnvironmentVariables.ROOT.Replace(@"\", "/"))
        {

        }

        [Newtonsoft.Json.JsonProperty("workspace")]
        public string Workspace { get; private set; }

        [Newtonsoft.Json.JsonProperty("parent")]
        public string Parent { get;  set; }
    }
}
