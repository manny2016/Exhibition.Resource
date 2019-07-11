


namespace Exhibition.Core.Services
{
    using System;
    using System.Windows.Forms;
    using Exhibition.Agent.Show;
    using Exhibition.Agent.Show.Models;
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
            try
            {
                AgentHost.TriggerDirectiveEvent(this, new OperationEventArgs() { Context = context });
                var copyof = context.DeepClone();
                copyof.Type = DirectiveTypes.Run;
                StoredState.Instance.Last[context.Directive.DefaultWindow.Id] = copyof;
                StoredState.Instance.Save();
            }
            catch (Exception ex) {

            }
            return new GeneralResponse<int>()
            {
                Success = true,
                Data = 1
            };
        }

        public void ShowLayoutInfo(Window[] windows)
        {
            AgentHost.TriggerShowLayoutInfoEvent(this, new LayoutinfoEventArgs(windows));
        }

        public void Shutdown()
        {
            Application.Exit();
        }

        public void UpgradeLayout(Window[] windows)
        {
            AgentHost.TriggerUpgardLayout(this, new LayoutinfoEventArgs(windows));
        }
    }
}
