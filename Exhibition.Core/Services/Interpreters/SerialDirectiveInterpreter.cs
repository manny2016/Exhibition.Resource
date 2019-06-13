

namespace Exhibition.Core.Services
{
    using System;
    using Models = Exhibition.Core.Models;
    using System.Text;
    using System.IO;
    using Newtonsoft.Json.Linq;

    public class SerialDirectiveInterpreter : DirectiveInterpreter
    {
        public SerialDirectiveInterpreter(IOperateContext directive)
            : base(directive)
        {

        }
        public override void Execute()
        {
            var settings = (this.Context.Directive.Terminal as SerialPortTerminal).Settings;
            var url = "http://localhost:8888/api/OperationService/Run";
            foreach (var resource in this.Context.Directive.Resources)
            {
                var fullName = resource.FullName.ServerMapFilePath();
                if (!File.Exists(fullName))
                    throw new FileNotFoundException(fullName);
                var hexString = File.ReadAllText(fullName).Split(' ');

                var result = url.GetUriJsonContent<JObject>((http) =>
                {
                    http.ContentType = "application/json";
                    http.Method = "POST";
                    using (var stream = http.GetRequestStream())
                    {
                        var data = new{ HexString = hexString, Settings = settings}.SerializeToJson();                        
                        var bytes = UTF8Encoding.Default.GetBytes(data);
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }
                    return http;
                });

            }
        }
    }
}
