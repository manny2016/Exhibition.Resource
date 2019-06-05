

namespace Exhibition.Core.Models
{
    public class GeneralResponse<T>
    {
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
