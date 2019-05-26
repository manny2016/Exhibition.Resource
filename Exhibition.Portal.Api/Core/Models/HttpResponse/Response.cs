

namespace Exhibition.Portal.Api.Models
{
    public abstract class Response<T>
        where T:class
    {
        public Response() {
            this.Success = true;
            this.ErrorCode = 0;
            this.ErrorMsg = string.Empty;
        }

        [Newtonsoft.Json.JsonProperty("data")]
        public T Data { get; set; }

        [Newtonsoft.Json.JsonProperty("success")]
        public bool Success { get; set; }

        [Newtonsoft.Json.JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [Newtonsoft.Json.JsonProperty("errorMsg")]
        public string ErrorMsg { get; set; }
    }
}
