

namespace Exhibition.Core.Models
{
    public class Directive
    {

        
        public string Name { get; set; }

        
        public string Description { get; set; }

        
        public DirectiveTypes Type { get; set; }

        
        public MediaPlayerTerminal Terminal { get; set; }

        
        public Window DefaultWindow { get; set; }

        
        public Resource[] Resources { get; set; }
    }

}
