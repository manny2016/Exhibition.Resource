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
    public class SQLiteQueryFilter<T>
    {
        [Newtonsoft.Json.JsonProperty("keys")]
        public T[] Keys { get; set; }

        [Newtonsoft.Json.JsonProperty("primaryKey")]
        public string PrimaryKey { get; set; }

        [Newtonsoft.Json.JsonProperty("search")]
        public string Search { get; set; }


        [Newtonsoft.Json.JsonProperty("terminalTypes")]
        public TerminalTypes[] TerminalTypes { get; set; }


        [Newtonsoft.Json.JsonProperty("directiveTypes")]
        public DirectiveTypes[] DirectiveTypes { get; set; }
    }
    public class SQLiteDimQueryFilter
    {
        [Newtonsoft.Json.JsonProperty("search")]
        public string Search { get; set; }
    }
}
