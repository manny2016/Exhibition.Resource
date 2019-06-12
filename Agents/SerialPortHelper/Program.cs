



namespace SerialPortHelper
{
    using Newtonsoft.Json.Linq;
    using SerialPortHelper.Services;
    using System;
    using System.IO;
    using System.Text;
    using System.Linq;
    using SerialPortHelper.Models;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            AgentHost.Configure((collection) =>
            {
                var url = "http://localhost:8080/api/mgr/QueryTerminals";
                var settings = url.GetUriJsonContent<JObject>((http) =>
                    {
                        http.Method = "POST";
                        http.ContentType = "application/json";
                        using (var stream = http.GetRequestStream())
                        {
                            var body = new { TerminalTypes = new int[] { 2 } }.SerializeToJson();
                            var buffers = UTF8Encoding.Default.GetBytes(body);
                            stream.Write(buffers, 0, buffers.Length);
                            stream.Flush();
                        }

                        return http;
                    }).SelectTokens("$.data[*].settings")
                  .Select(ctx =>
                  {
                      return ctx.ToString().DeserializeToObject<SerialPortSettings>();
                  });
                collection.AddSingleton(typeof(IEnumerable<SerialPortSettings>), settings);
                collection.AddSingleton(typeof(SerialPortService));
            });            
            AgentHost.GetService<SerialPortService>()
                .Run();
        }

        
    }
}
