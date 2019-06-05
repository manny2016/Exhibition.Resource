using System;
using System.Collections.Generic;
using System.Text;

namespace Exhibition.Core
{
    public interface ITerminal<T> : IBaseTerminal
        where T : class
    {
        [Newtonsoft.Json.JsonProperty("settings")]
        T Settings { get; }
    }
}
