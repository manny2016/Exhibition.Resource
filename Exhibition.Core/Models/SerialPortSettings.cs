

namespace Exhibition.Core
{
    using System.IO.Ports;
    public class SerialPortSettings 
    {
        
        /// <summary>
        /// 串口号
        /// </summary>
        [Newtonsoft.Json.JsonProperty("SerialPort")]
        public string SerialPort{ get; set; }

        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        [Newtonsoft.Json.JsonProperty("baudRate")]
        public int BaudRate { get; set; }

        /// <summary>
        /// 校验位
        /// </summary>
        [Newtonsoft.Json.JsonProperty("parity")]
        public System.IO.Ports.Parity Parity { get; set; }        

        /// <summary>
        /// 停止位
        /// </summary>
        [Newtonsoft.Json.JsonProperty("stopBits ")]
        public System.IO.Ports.StopBits StopBits { get; set; }

    }
}
