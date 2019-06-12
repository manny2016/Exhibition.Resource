


namespace Exhibition.Core.Services
{
    using System.Windows.Forms;
    using Exhibition.Agent.Show;
    using Exhibition.Core.Models;
    using OperationContext = Exhibition.Agent.Show.Models.OperationContext;
    public class OperationService : IOperationService
    {
        public string Readme()
        {
            return "this is a wcf api for control center.";
        }

        public GeneralResponse<int> Run(OperationContext context)
        {
            AgentHost.TriggerDirectiveEvent(this, new OperationEventArgs() { Context = context });
            return new GeneralResponse<int>()
            {
                Success = true,
                Data = 1
            };
        }

        public void Shutdown()
        {
            Application.Exit();
        }
    }
}
