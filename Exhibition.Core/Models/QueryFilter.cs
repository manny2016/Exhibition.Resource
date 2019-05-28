using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exhibition.Core.Models
{
    public class QueryFilter
    {
        /// <summary>
        /// 当前资源相对路径
        /// </summary>
        [Newtonsoft.Json.JsonProperty("current")]
        public string Current { get; set; }

        [Newtonsoft.Json.JsonProperty("search")]
        public string Search { get; set; }
    }
    public class SQLiteQueryFilter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
