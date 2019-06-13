using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SerialPortHelper.Models;
using OperationContext = SerialPortHelper.Models.OperationContext;

namespace SerialPortHelper.Services
{
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class OperationService : IOperationService
    {

        public GeneralResponse<string> Readme()
        {
            return new GeneralResponse<string>()
            {
                Data = "I am REST Api"
            };
        }

        public GeneralResponse<int> Run(OperationContext context)
        {
            var service = AgentHost.GetService<SerialPortService>();
            var buffers = context.HexString.Select((ctx) =>
            {
                return byte.Parse(ctx, System.Globalization.NumberStyles.HexNumber);
            }).ToArray();
            service.Send(context.Settings, buffers);
            return new GeneralResponse<int>();
        }


    }
}
