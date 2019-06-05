
using System.Windows.Forms;

using Exhibition.Core;
using Exhibition.Core.Models;

namespace Exhibition.Components
{
    public partial class VideoPlayer : UserControl, IOperate
    {
        private static VideoPlayer instance;
        private static object lockObject = new object();


        public static VideoPlayer CreateVideoPlayer(Resource resource)
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        instance = new VideoPlayer(resource);
                    }
                }
            }
            return instance;
        }

        private VideoPlayer(Resource resource)
        {
            InitializeComponent();
            this.Load += VideoPlayer_Load;
        }

        private void VideoPlayer_Load(object sender, System.EventArgs e)
        {
            this.Width = this.Parent.Width;
            this.Height = this.Parent.Height;
        }

        public void Play(Resource resource)
        {
          
        }


        public void Stop()
        {
            
        }
    
    }
}
