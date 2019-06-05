


namespace Exhibition.Components
{
    using System.Windows.Forms;
    using Exhibition.Core.Models;
    using Chromium.WebBrowser;
    using Chromium.Remote;

    using Exhibition.Core;
    using System.IO;

    public partial class AxWebBrowser : UserControl, IOperate
    {

        public AxWebBrowser(Resource resource)
        {
            this.InitializeComponent();
            this.Load += AxWebBrowser_Load;
            this.InitializeChromiumWebBrowser();
        }

        private void AxWebBrowser_Load(object sender, System.EventArgs e)
        {
            this.Width = this.Parent.Width;
            this.Height = this.Parent.Height;
            this.WebBrowser.Width = this.Width;
            this.WebBrowser.Height = this.Height;
        }

        CfrBrowser remoteBrowser;
        public Chromium.WebBrowser.ChromiumWebBrowser WebBrowser;
        private void InitializeChromiumWebBrowser()
        {
            this.SuspendLayout();
            this.WebBrowser = new ChromiumWebBrowser();
            this.WebBrowser.BackColor = System.Drawing.Color.White;
            this.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebBrowser.Location = new System.Drawing.Point(0, 0);
            this.WebBrowser.Name = "WebBrowser";
            this.WebBrowser.RemoteCallbackInvokeMode = Chromium.WebBrowser.JSInvokeMode.Inherit;
            this.WebBrowser.Size = new System.Drawing.Size(1441, 605);
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
            using (var stream = new FileStream(resource.FullName, FileMode.Open, FileAccess.Read))
            {
                var reader = new StreamReader(stream);
                var url = reader.ReadToEnd();
                this.WebBrowser.LoadUrl(url);
            }
        }

        public void Stop()
        {
            if (this.IsDisposed == false)
            {
                this.Parent.Controls.Remove(this);
                base.Dispose(true);
            }
        }

    }
}
