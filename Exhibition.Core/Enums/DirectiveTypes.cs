using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exhibition.Core
{
    public enum DirectiveTypes
    {
        PowerOff = 1,
        PowerOn = 2,
        PlayVideo = 3,
        PlayImages = 4,
        ChangePlayMode = 5,
        Restart = 6,
        CloseMonitor = 7,
        StopcCurrentPlayTask = 8,

    }
}
