using System;
using System.Collections.Generic;
using System.Text;

namespace Exhibition.Core
{
    public interface IBaseTerminal
    {
        
        string Name { get; }

        
        string Description { get; }

        
        TerminalTypes Type { get; }

        string GetSettings();
    }
}
