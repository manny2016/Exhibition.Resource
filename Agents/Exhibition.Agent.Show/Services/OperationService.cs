using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exhibition.Agent.Show;
using Exhibition.Core.Models;

namespace Exhibition.Core.Services
{
    public class OperationService : IOperationService
    {
        public GeneralResponse<int> Run(Directive directive)
        {
            AgentHost.TriggerDirectiveEvent(this, directive);
            return new GeneralResponse<int>()
            {
                Success = true,
                Data = 1
            };
        }
    }
}
