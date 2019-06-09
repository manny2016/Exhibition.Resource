using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exhibition.Core
{
    public enum DirectiveTypes
    {
        /// <summary>
        /// 执行播放，开关机,音量调节等指令
        /// </summary>
        Run = 1,
        /// <summary>
        /// 
        /// </summary>
        Stop = 2,
        /// <summary>
        /// 上一个
        /// </summary>
        Next = 3,
        /// <summary>
        /// 下一个
        /// </summary>
        Previous = 4,

        /// <summary>
        /// 切换播放模式
        /// </summary>
        SwitchModel = 5,
        /// <summary>
        /// 向上滚动
        /// </summary>
        ScrollUp = 6,
        /// <summary>
        /// 向下滚动
        /// </summary>
        ScrollDown = 7
    }
}
