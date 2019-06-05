using System;
using System.Collections.Generic;
using System.Text;

namespace Exhibition.Core
{
    public interface IBaseTerminal
    {
        [Newtonsoft.Json.JsonProperty("name")]
        string Name { get; }

        [Newtonsoft.Json.JsonProperty("description")]
        string Description { get; }

        [Newtonsoft.Json.JsonProperty("type")]
        TerminalTypes Type { get; }

        string GetSettings();
    }
}
