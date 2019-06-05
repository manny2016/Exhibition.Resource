using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Exhibition.Core.Models
{
    public class Window
    {
        
        public int Id { get; set; }

        
        public WinPoint Location { get; set; }

        
        public WinSize Size { get; set; }

        
        public int Monitor { get; set; }
    }

    public class WinSize
    {
        
        public int Width { get; set; }
        
        public int Height { get; set; }
    }
    public class WinPoint {
        
        public int X { get; set; }
        
        public int Y { get; set; }
    }
}
