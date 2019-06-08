

namespace Exhibition.Core.Services
{
    using System;
    using System.Text;
    using Models = Exhibition.Core.Models;
    public class MediaDirectiveInterpreter : DirectiveInterpreter
    {
        public MediaDirectiveInterpreter(IOperateContext directive)
            : base(directive)
        {

        }

        public override void Execute()
        {
            try
            {
                var url = (this.Context.Directive.Terminal as Models::MediaPlayerTerminal)?.Settings.Endpoint;
                url.GetUriJsonContent<Models::GeneralResponse<int>>((http) =>
                {
                    http.Method = "POST";
                    http.ContentType = "application/json";
                    using (var stream = http.GetRequestStream())
                    {
                        var body =this.Context.SerializeToJson();
                        var buffers = UTF8Encoding.Default.GetBytes(body);
                        stream.Write(buffers, 0, buffers.Length);
                        stream.Flush();
                    }
                    return http;
                });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
