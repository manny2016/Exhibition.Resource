using Exhibition.Core.Models;
using System;
using ShowModels = Exhibition.Agent.Show.Models;


namespace Exhibition.Agent.Show
{
    public delegate void OperationEventHandler(object sender, OperationEventArgs e);
    public delegate void LayoutInfoEventHandler(object sender, LayoutinfoEventArgs e);
    public class OperationEventArgs : EventArgs
    {
        public ShowModels::OperationContext Context { get; set; }
    }
    public class LayoutinfoEventArgs
    {
        public LayoutinfoEventArgs(Window[] windows)
        {
            this.Windows = windows;
        }
        public Window[] Windows { get; private set; }
    }
}
