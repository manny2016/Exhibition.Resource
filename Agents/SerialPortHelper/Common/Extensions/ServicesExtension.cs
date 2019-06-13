
namespace SerialPortHelper
{
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Linq;
    using SerialPortHelper.Models;
    using SerialPortHelper.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    public static class ServicesExtension
    {
        const string WorkflowConfigurationFile = "workflow.json";
        public static void AddSerialPortService(this IServiceCollection collection)
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
            var workflow = new WorkflowDescriptor();
            if (!File.Exists(WorkflowConfigurationFile))
            {
                workflow = new WorkflowDescriptor()
                {
                    Endpoint = "http://localhost:8080/api/mgr/Execute",
                    Descriptors = new Descriptor[] {
                          new Descriptor(){  Condition="01", Context = new OperationContextforApi(){
                               Name = "S-01",
                                Type =1
                          } }
                      }
                };
                using (var stream = new FileStream(WorkflowConfigurationFile, FileMode.Create))
                {
                    var writer = new StreamWriter(stream);
                    writer.Write(workflow.SerializeToJson());
                    writer.Flush();
                }
            }
            else
            {
                workflow = File.ReadAllText(WorkflowConfigurationFile).DeserializeToObject<WorkflowDescriptor>();
            }
            collection.AddSingleton(typeof(WorkflowDescriptor), workflow);
            collection.AddSingleton(typeof(IEnumerable<SerialPortSettings>), settings);
            collection.AddSingleton(typeof(SerialPortService));
        }
    }
}
