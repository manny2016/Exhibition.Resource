

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

    public delegate void RunDirectiveDelegate(object sender, ShowModels::OperationContext context);
    public partial class ForumMain : Form
    {
        public ForumMain()
        {
            InitializeComponent();
            this.Load += ForumMain_Load;
            this.LoadWindowConfiguration();
            this.KeyPreview = true;
            this.KeyUp += ForumMain_KeyUp;

        }

        private void ForumMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                Application.Exit();
            }
        }

        private MediaPlayerTerminal terminal;
        private Dictionary<string, WorkingState> states = new Dictionary<string, WorkingState>();
        private void ForumMain_Load(object sender, EventArgs e)
        {
            AgentHost.DirectiveReceived += AgentHost_DirectiveReceived;
            this.FixWindowLocationByMonitor();
        }


        private void FixWindowLocationByMonitor()
        {
            var screens = Screen.AllScreens;
            this.SetBounds(screens.Min(o => o.Bounds.X), screens.Min(o => o.Bounds.Y),
                screens.Sum(o => o.Bounds.Width), screens.Max(o => o.Bounds.Height));

        }
        private void LoadWindowConfiguration()
        {
            try
            {
                var url = string.Format(AgentHost.Api, "QueryTerminals");
                var result = url.GetUriJsonContent<GeneralResponse<MediaPlayerTerminal[]>>((http) =>
                  {
                      http.Method = "POST";
                      http.ContentType = "application/json; charset=utf-8";
                      var data = new
                      {
                          Keys = new string[] { AgentHost.TerminalName },
                          PrimaryKey = "Name",
                          TerminalTypes = new int[] { (int)TerminalTypes.MediaPlayer }
                      };
                      using (var stream = http.GetRequestStream())
                      {
                          var body = data.SerializeToJson();
                          var buffers = UTF8Encoding.Default.GetBytes(body);
                          stream.Write(buffers, 0, buffers.Length);
                          stream.Flush();
                      }


                      return http;
                  });
                this.terminal = result.Data.FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
        }
        private void AgentHost_DirectiveReceived(object sender, OperationEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new RunDirectiveDelegate(this.Run), sender,e.Context);
            }
            else
            {
                this.Run(sender, e.Context);
            }
        }
        private void Run(object sender, Show.Models.OperationContext context)
        {
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
            RemovePlayerforNewDirective(directive.Name,directive.DefaultWindow.Id);
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

        private void RemovePlayerforNewDirective(string name,int iWindowId)
        {
            if (states.ContainsKey(name)==false)
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
    }
}
