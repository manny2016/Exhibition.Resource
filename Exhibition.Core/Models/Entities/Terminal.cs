

namespace Exhibition.Core.Entities
{
    public class Terminal
    {
        
        public virtual string Name { get; set; }

        
        public virtual string Description { get; set; }

        
        public virtual TerminalTypes Type { get; set; }


        public virtual string Settings { get; set; }
    }
}
