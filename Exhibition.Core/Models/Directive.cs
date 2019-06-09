

namespace Exhibition.Core.Models
{
    using Newtonsoft.Json;
    public class Directive
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
        public IBaseTerminal Terminal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("DefaultWindow",NullValueHandling = NullValueHandling.Ignore)]
        public Window DefaultWindow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Resource[] Resources { get; set; }
    }

    public class DirectiveforTransmit
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
        public string Terminal { get; set; }        
        
        public Window DefaultWindow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Resource[] Resources { get; set; }
    }

}
