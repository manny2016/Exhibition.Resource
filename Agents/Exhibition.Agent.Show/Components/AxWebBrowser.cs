


namespace Exhibition.Components
{
    using System.Windows.Forms;
    using Exhibition.Core.Models;
    using Chromium.WebBrowser;
    using Chromium.Remote;

    using Exhibition.Core;
    using System.IO;
    using Exhibition.Agent.Show;
    using System.Drawing;
    using System.Collections.Generic;
    using System.Linq;
    public partial class AxWebBrowser : UserControl, IOperate
    {
        private LinkedList<Resource> linked = null;
        private LinkedListNode<Resource> current = null;
        public AxWebBrowser(Resource[] resources, string name)
        {
            linked = new LinkedList<Resource>(resources.OrderBy(o => o.Name));
            current = linked.First;
            this.InitializeComponent();
            this.Load += AxWebBrowser_Load;
            this.InitializeChromiumWebBrowser(name);
        }

        private void AxWebBrowser_Load(object sender, System.EventArgs e)
        {

            this.WebBrowser.Width = this.Width;
            this.WebBrowser.Height = this.Height;
        }

        CfrBrowser remoteBrowser;
        public Chromium.WebBrowser.ChromiumWebBrowser WebBrowser;
        private void InitializeChromiumWebBrowser(string name)
        {
            this.SuspendLayout();
            this.WebBrowser = new ChromiumWebBrowser();
            this.WebBrowser.BackColor = System.Drawing.Color.White;
            //this.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.WebBrowser.Location = new System.Drawing.Point(0, 0);
            this.WebBrowser.Name = name;
            this.WebBrowser.RemoteCallbackInvokeMode = Chromium.WebBrowser.JSInvokeMode.Inherit;
            //this.WebBrowser.Size = new System.Drawing.Size(1441, 605);
            this.WebBrowser.TabIndex = 2;
            this.WebBrowser.RemoteBrowserCreated += (s, e) =>
            {
                remoteBrowser = e.Browser;
            };
            this.Controls.Add(this.WebBrowser);
            this.ResumeLayout(false);
        }

        public void Play(Resource resource)
        {

            var url = AgentHost.Resource + "/" + this.current.Value.FullName;            
            this.WebBrowser.LoadUrl(url);
        }

        public void Stop()
        {
            if (this.IsDisposed == false)
            {
                this.Parent.Controls.Remove(this);
                base.Dispose(true);
            }
        }

        public void Next()
        {
            this.current = this.current.Next ?? linked.Last;
            this.Play(this.current.Value);
        }

        public void Previous()
        {
            this.current = this.current.Previous?? linked.First;
            this.Play(this.current.Value);
        }
    }
}
