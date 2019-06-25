

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
            foreach (var address in Dns.GetHostAddresses(Dns.GetHostName()).Where(o => o.AddressFamily == AddressFamily.InterNetwork))
            {
                foreach (var terminal in result.Data)
                {
                    if (terminal.Settings == null || string.IsNullOrEmpty(terminal.Settings.Endpoint)) continue;
                    if (terminal.Settings.Endpoint.IndexOf(address.ToString()) >= 0)
                    {
                        foreach (var idx in terminal.Settings.Windows.GroupBy(o => o.Monitor).Select(o => o.Key))
                        {
                            states[idx] = new FromWapper(idx);
                            states[idx].Show();
                        }
                        return;
                    }
                }
            }
        }

    }
}
