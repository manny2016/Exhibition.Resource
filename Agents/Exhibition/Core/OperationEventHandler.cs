using ShowModels = Exhibition.Agent.Show.Models;


namespace Exhibition.Agent.Show
{
    public delegate void OperationEventHandler(object sender, OperationEventArgs e);
    public class OperationEventArgs
    {
        public ShowModels::OperationContext Context { get; set; }
    }
}
