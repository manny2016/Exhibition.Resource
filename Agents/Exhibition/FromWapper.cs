
namespace Exhibition.Agent.Show
{
    using System;
    using System.Windows.Forms;
    using Exhibition.Core.Models;
    using Exhibition.Core;
    using System.Text;
    using System.Linq;
    using Models = Exhibition.Core.Models;
    using ShowModels = Exhibition.Agent.Show.Models;
    using System.Collections.Generic;
    using Exhibition.Agent.Show.Models;
    using Exhibition.Components;
    using System.Drawing;

    public partial class FromWapper : Form
    {

        private FromWapper()
        {
            InitializeComponent();
            this.Load += FromWapper_Load;
        }
        public int DefaultMonitor { get; private set; }
        public Window[] Windows { get; set; }
        public FromWapper(int monitor, Window[] windows) : this()
        {
            this.DefaultMonitor = monitor;
            this.Windows = windows;

        }
        private void FromWapper_Load(object sender, EventArgs e)
        {
            AgentHost.DirectiveReceived += AgentHost_DirectiveReceived;
            this.Locating();
            foreach (var window in this.Windows)
            {
                if (StoredState.Instance.Last.ContainsKey(window.Id))
                {
                    var context = StoredState.Instance.Last[window.Id];
                    AgentHost.TriggerDirectiveEvent(this, new OperationEventArgs() { Context = context });
                }
            }

        }
        private void Locating()
        {
            if (this.DefaultMonitor >= Screen.AllScreens.Length) return;
            var screen = Screen.AllScreens[this.DefaultMonitor];
            this.Location = new Point(screen.Bounds.X, screen.Bounds.Y);
            this.Top = 0;
            this.Left = screen.WorkingArea.Left;
            this.Width = screen.WorkingArea.Width;
            this.Height = screen.WorkingArea.Height;
        }
        private Dictionary<string, WorkingState> states = new Dictionary<string, WorkingState>();
        private void AgentHost_DirectiveReceived(object sender, OperationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new RunDirectiveDelegate(this.Run), sender, e.Context);
            }
            else
            {
                this.Run(sender, e.Context);
            }
        }
        private void Run(object sender, Show.Models.OperationContext context)
        {
            if (context.Directive.DefaultWindow.Monitor != this.DefaultMonitor) return;
            if (context.Directive.Resources.Length.Equals(0)) return;
            var name = context.Directive.Name;
            switch (context.Type)
            {
                case DirectiveTypes.Next:
                    if (states.ContainsKey(name))
                    {
                        states[name].Operator.Next();
                    }
                    break;
                case DirectiveTypes.Previous:
                    if (states.ContainsKey(name))
                    {
                        states[name].Operator.Previous();
                    }
                    break;
                case DirectiveTypes.Run:
                    GenernateOperator(context.Directive)?.Play(context.Directive.Resources[0]);
                    break;
                case DirectiveTypes.Stop:
                    states[context.Directive.Name]?.Operator.Stop();
                    break;
                case DirectiveTypes.SwitchModel:
                    states[context.Directive.Name]?.Operator.SwichMode();
                    break;
                case DirectiveTypes.ScrollDown:
                    states[context.Directive.Name]?.Operator.ScrollDown();
                    break;
                case DirectiveTypes.ScrollUp:
                    states[context.Directive.Name]?.Operator.ScrollUp();
                    break;
            }
        }
        public IOperate GenernateOperator(ShowModels::MediaControlDirective directive)
        {
            RemovePlayerforNewDirective(directive.Name, directive.DefaultWindow.Id);
            if (!states.ContainsKey(directive.Name))
            {
                var state = new WorkingState()
                {
                    Id = directive.DefaultWindow.Id,
                    Name = directive.Name,
                    Window = directive.DefaultWindow,
                    Resources = directive.Resources,
                    Current = 0
                };
                state.Operator = CreatePlayer(state.Type, directive.Name, state.Window, directive.Resources);
                states[directive.Name] = state;
            }
            return states[directive.Name].Operator;
        }

        private IOperate CreatePlayer(ResourceTypes type, string name, Window window, Resource[] resources)
        {
            UserControl control = null;
            switch (type)
            {
                case ResourceTypes.TextPlain:
                case ResourceTypes.Image:
                case ResourceTypes.Video:
              
                    control = new AxWebBrowser(resources, name);
                    break;
                case ResourceTypes.Folder:
                default:
                    throw new NotSupportedException(type.ToString());
            }
            this.SuspendLayout();
            control.Width = window.Size.Width;
            control.Height = window.Size.Height;
            control.Location = new System.Drawing.Point(window.Location.X, window.Location.Y);
            this.Controls.Add(control);
            this.ResumeLayout(false);
            return control as IOperate;
        }

        private void RemovePlayerforNewDirective(string name, int iWindowId)
        {
            if (states.ContainsKey(name) == false)
            {
                foreach (var state in states)
                {
                    if (state.Value.Id == iWindowId)
                    {
                        state.Value.Operator?.Stop();
                        states.Remove(state.Key);
                        return;
                    }
                }
            }
        }
        public void UpgradeLayout(Window[] windows)
        {
            this.Windows = windows;
            foreach (var window in windows)
            {
                var state = this.states.FirstOrDefault(o => o.Value.Id == window.Id);
                if (state.Value != null)
                {
                    state.Value.Window = window;
                }
            }
        }
    }
}
