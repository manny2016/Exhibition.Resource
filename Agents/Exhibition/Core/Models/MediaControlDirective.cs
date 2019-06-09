using Exhibition.Core;
using Exhibition.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhibition.Agent.Show.Models
{
    public class MediaControlDirective
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("DefaultWindow", NullValueHandling = NullValueHandling.Ignore)]
        public Window DefaultWindow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Resource[] Resources { get; set; }
    }
}
