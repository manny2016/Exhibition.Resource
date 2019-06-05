

namespace Exhibition.Agent.Show
{
    using System;
    using System.Windows.Forms;
    using Exhibition.Core.Models;
    using Exhibition.Core;
    using System.IO;
    using System.Text;
    using System.Linq;
    using Models = Exhibition.Core.Models;
    using System.Collections.Generic;
    using Exhibition.Agent.Show.Models;
    using Exhibition.Components;

    public partial class ForumMain : Form
    {
        public ForumMain()
        {
            InitializeComponent();
            this.Load += ForumMain_Load;
            this.LoadWindowConfiguration();
        }
        private MediaPlayerTerminal terminal;
        private Dictionary<string, WorkingState> states = new Dictionary<string, WorkingState>();
        private void ForumMain_Load(object sender, EventArgs e)
        {
            AgentHost.DirectiveReceived += AgentHost_DirectiveReceived;

        }
        private void LoadWindowConfiguration()
        {
            var url = string.Format(AgentHost.Api, "QueryTerminals");
            var result = url.GetUriJsonContent<GeneralResponse<MediaPlayerTerminal[]>>((http) =>
              {
                  http.Method = "POST";
                  http.ContentType = "application/json";
                  using (var stream = new StreamWriter(http.GetRequestStream(), System.Text.Encoding.UTF8))
                  {
                      var body = new
                      {
                          Keys = new string[] { AgentHost.TerminalName },
                          PrimaryKey = "Name",
                          TerminalTypes = new int[] { (int)TerminalTypes.MediaPlayer }
                      };
                      var json = body.SerializeToJson();
                      stream.Write(json);
                      stream.Flush();
                  }
                  return http;
              });
            this.terminal = result.Data.FirstOrDefault();
        }
        private void AgentHost_DirectiveReceived(object sender, Models::Directive directive)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new OperationEventHandler(this.Run), sender, directive);
            }
            else
            {

            }
        }
        private void Run(object sender, Models::Directive directive)
        {
            switch (directive.Type)
            {
                case DirectiveTypes.Next:
                    break;
                case DirectiveTypes.Previous:
                    break;
                case DirectiveTypes.Run:
                    break;
                case DirectiveTypes.Stop:
                    states[directive.Name]?.Operator.Stop();
                    break;
            }
        }
        public IOperate GenernateOperator(Models::Directive directive)
        {
            RemovePlayerforNewDirective(directive);
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
                state.Operator = CreatePlayer(state.Type, state.Window, directive.Resources[0]);
            }
            return states[directive.Name].Operator;
        }

        private IOperate CreatePlayer(ResourceTypes type, Window window, Resource resource)
        {
            UserControl control = null;
            switch (type)
            {
                case ResourceTypes.H5:
                    control = new AxWebBrowser(resource);
                    break;
                case ResourceTypes.Image:
                    control = new ImagePlayer(resource);
                    break;
                case ResourceTypes.Video:
                    control = new DSMediaPlayer(resource);
                    break;
                case ResourceTypes.Folder:
                case ResourceTypes.SerialPortDirective:
                default:
                    throw new NotSupportedException(type.ToString());
            }
            this.SuspendLayout();
            control.Width = window.Size.Width;
            Height = window.Size.Height;
            control.Location = new System.Drawing.Point(window.Location.X, window.Location.Y);
            this.Controls.Add(control);
            this.ResumeLayout(false);
            return control as IOperate;
        }

        private void RemovePlayerforNewDirective(Models::Directive directive)
        {
            if (!states.ContainsKey(directive.Name))
            {
                foreach (var state in states)
                {
                    if (state.Value.Id == directive.DefaultWindow.Id)
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
