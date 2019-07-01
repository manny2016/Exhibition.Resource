

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
    using System.Net.Sockets;
    using System.Net.NetworkInformation;
    using System.Net;

    public delegate void RunDirectiveDelegate(object sender, ShowModels::OperationContext context);
    public partial class ForumMain : Form
    {
        public ForumMain()
        {
            InitializeComponent();
            this.Load += ForumMain_Load;
            AgentHost.ShowLayoutInfo += AgentHost_ShowLayoutInfo;
            AgentHost.UpgradeLayoutInfo += AgentHost_UpgradeLayoutInfo;
            this.KeyPreview = true;
            this.KeyUp += ForumMain_KeyUp;

        }

        private void AgentHost_UpgradeLayoutInfo(object sender, LayoutinfoEventArgs e)
        {
            if (this.InvokeRequired)
            {
                LayoutInfoEventHandler handler = new LayoutInfoEventHandler(AgentHost_UpgradeLayoutInfo);
                handler.Invoke(sender, e);
            }
            else
            {
                var monitors = e.Windows.Select(o => o.Monitor).Distinct();              
                foreach (var idx in monitors)
                {
                    if (this.states.ContainsKey(idx))
                    {
                        states[idx].UpgradeLayout(e.Windows.Where(o => o.Monitor == idx).ToArray());
                    }
                
                }
            }
        }

        private void AgentHost_ShowLayoutInfo(object sender, LayoutinfoEventArgs e)
        {
            if(this.InvokeRequired)
            {
              LayoutInfoEventHandler handler = new LayoutInfoEventHandler(this.AgentHost_ShowLayoutInfo);
                this.Invoke(handler, sender, e);
            }
            else
            {
                var monitors = e.Windows.Select(o => o.Monitor).Distinct();
                foreach (var monitor in monitors)
                {
                    var info = new FrmLayoutInfo(monitor, e.Windows.Where(o => o.Monitor == monitor).ToArray());
                    info.Show();
                }
            }
          
        }

        private void ForumMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                Application.Exit();
            }
        }



        private void ForumMain_Load(object sender, EventArgs e)
        {
            this.LoadWindowConfiguration();
        }

        Dictionary<int, FromWapper> states = new Dictionary<int, FromWapper>();

        private void LoadWindowConfiguration()
        {

            var url = string.Format(AgentHost.Api, "QueryTerminals");
            var result = url.GetUriJsonContent<GeneralResponse<MediaPlayerTerminal[]>>((http) =>
              {
                  http.Method = "POST";
                  http.ContentType = "application/json; charset=utf-8";
                  var data = new
                  {
                      TerminalTypes = new TerminalTypes[] { TerminalTypes.MediaPlayer }
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
            if (result.Data.Length == 0) throw new ArgumentOutOfRangeException("cant find any terminals configuration from server");
            var address = Dns.GetHostAddresses(Dns.GetHostName()).Where(o => o.AddressFamily == AddressFamily.InterNetwork)
                .Where(o => !string.IsNullOrEmpty(o.ToString())).Select(o => o.ToString());

            var terminal = result.Data.FirstOrDefault(o => address.Any(add => o.Settings.Endpoint.IndexOf(add) >= 0));
            if (terminal != null)
            {
                var monitors = terminal.Settings.Windows.Select(o => o.Monitor).Distinct();
                foreach (var idx in monitors)
                {
                    states[idx] = new FromWapper(idx, terminal.Settings.Windows.Where(o => o.Monitor == idx).ToArray());
                    states[idx].Show();
                }
            }

        }

    }
}
