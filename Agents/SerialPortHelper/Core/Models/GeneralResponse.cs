using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortHelper.Models
{
    public class GeneralResponse<T>
    {
        public GeneralResponse()
        {
            this.Success = true;
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
