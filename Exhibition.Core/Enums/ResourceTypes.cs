using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exhibition.Core
{
    /// <summary>
    /// 
    /// </summary>
    public enum ResourceTypes
    {
        ////TODO  P3 need identity file by H5 and SerialPortDirective
        Folder = 1,
        Video = 2,
        Image = 3,
        TextPlain = 4,
        
        //H5 = 4,
        //SerialPortDirective = 5,
        NotSupported = 6,
    }
}
