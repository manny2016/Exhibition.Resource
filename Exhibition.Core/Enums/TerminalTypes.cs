using System;
using System.Collections.Generic;
using System.Text;

namespace Exhibition.Core
{
    public enum TerminalTypes
    {
        NotSupport=-1,
        /// <summary>
        /// 播放器终端
        /// </summary>
        MediaPlayer = 1,
        /// <summary>
        /// 串口终端
        /// </summary>
        SerialPort = 2,
    }
}
