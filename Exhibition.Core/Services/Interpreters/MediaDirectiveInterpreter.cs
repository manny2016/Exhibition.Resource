

namespace Exhibition.Core.Services
{
    using System;
    using System.Text;
    using Models = Exhibition.Core.Models;
    public class MediaDirectiveInterpreter : DirectiveInterpreter
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(MediaDirectiveInterpreter));
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
                        var body = this.Context.SerializeToJson();
                        var buffers = UTF8Encoding.Default.GetBytes(body);
                        stream.Write(buffers, 0, buffers.Length);
                        stream.Flush();
                    }
                    return http;
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Execute error:Directive Context:{this.Context.SerializeToJson()},{ex.SerializeToJson()}");
            }
        }
    }
}
