using System;

using System.Windows.Forms;

using DirectShowLib;
using Exhibition.Core;
using Exhibition.Core.Models;

namespace Exhibition.Components
{
    public partial class DSMediaPlayer : UserControl, IOperate
    {
        private static DSMediaPlayer instance;
        private static object lockObject = new object();


        //public static DSMediaPlayer CreateDSMediaPlayer(Resource resource)
        //{
        //    lock (lockObject)
        //    {
        //        if (instance == null)
        //        {
        //            lock (lockObject)
        //            {
        //                instance = new DSMediaPlayer(resource);
        //            }
        //        }
        //    }
        //    return instance;
        //}
        public DSMediaPlayer(Resource resource)
        {
            InitializeComponent();
            this.InitDirectShowLib();
            this.Load += DSMediaPlayer_Load;
        }

        private void DSMediaPlayer_Load(object sender, EventArgs e)
        {
            this.Width = this.Parent.Width;
            this.Height = this.Parent.Height;
            this.panel1.Width = this.Width;
            this.panel1.Height = this.Height;
        }

        private string mediaFile = string.Empty;

        private const int WMGraphNotify = 0x0400 + 13;
        private const int VolumeFull = 0;
        private const int VolumeSilence = -10000;
        private IVideoWindow videoWindow = null;
        private IGraphBuilder graphBuilder = null;
        private IMediaControl mediaControl = null;
        private IMediaEventEx mediaEventEx = null;

        private IBasicAudio basicAudio = null;
        private IBasicVideo basicVideo = null;
        private IMediaSeeking mediaSeeking = null;
        private IMediaPosition mediaPosition = null;
        private IVideoFrameStep frameStep = null;
        private string sMediaFile = string.Empty;
        private bool isAudioOnly = false;
        private bool isFullScreen = false;
        private int currentVolume = VolumeFull;

        private double currentPlaybackRate = 1.0;
        bool bStoping = false;
        private IntPtr hDrain = IntPtr.Zero;
        private int hr = 0;

        /// <summary>
        /// 当前视频文件
        /// </summary>
        public string MediaFile { get { return this.mediaFile; } }


        /// <summary>
        /// 初始化DirectShow
        /// </summary>
        private void InitDirectShowLib()
        {


        }










        //private void MoveVideoWindow()
        //{
        //    int hr = 0;

        //    // Track the movement of the container window and resize as needed
        //    if (this.videoWindow != null)
        //    {
        //        hr = this.videoWindow.SetWindowPosition(
        //          0,
        //          0,
        //          this.Width,
        //          this.Height);
        //        DsError.ThrowExceptionForHR(hr);
        //    }
        //}

        private void CheckVisibility()
        {
            int hr = 0;
            OABool lVisible;

            if ((this.videoWindow == null) || (this.basicVideo == null))
            {
                // Audio-only files have no video interfaces.  This might also
                // be a file whose video component uses an unknown video codec.
                this.isAudioOnly = true;
                return;
            }
            else
            {
                // Clear the global flag
                this.isAudioOnly = false;
            }

            hr = this.videoWindow.get_Visible(out lVisible);
            if (hr < 0)
            {
                // If this is an audio-only clip, get_Visible() won't work.
                //
                // Also, if this video is encoded with an unsupported codec,
                // we won't see any video, although the audio will work if it is
                // of a supported format.
                if (hr == unchecked((int)0x80004002)) //E_NOINTERFACE
                {
                    this.isAudioOnly = true;
                }
                else
                    DsError.ThrowExceptionForHR(hr);
            }
        }
        private string currentMediaFile = string.Empty;
        public void Play(Resource resource)
        {
            // Have the graph builder construct its the appropriate graph automatically
            if (this.currentMediaFile.Equals(resource.FullName))
                return;
            this.graphBuilder = (IGraphBuilder)new FilterGraph();
            hr = this.graphBuilder.RenderFile(resource.FullName, null);
            DsError.ThrowExceptionForHR(hr);
            // QueryInterface for DirectShow interfaces
            this.mediaControl = (IMediaControl)this.graphBuilder;
            this.mediaEventEx = (IMediaEventEx)this.graphBuilder;
            this.mediaSeeking = (IMediaSeeking)this.graphBuilder;
            this.mediaPosition = (IMediaPosition)this.graphBuilder;

            // Query for video interfaces, which may not be relevant for audio files
            this.videoWindow = this.graphBuilder as IVideoWindow;
            this.basicVideo = this.graphBuilder as IBasicVideo;

            // Query for audio interfaces, which may not be relevant for video-only files
            this.basicAudio = this.graphBuilder as IBasicAudio;
            this.MoveVideoWindow();
            this.mediaControl.Run();
            this.currentMediaFile = resource.FullName;
        }
        private void MoveVideoWindow()
        {
            int hr = 0;

            // Track the movement of the container window and resize as needed
            if (this.videoWindow != null)
            {
                this.videoWindow.put_WindowState(WindowState.Normal);
                this.videoWindow.put_WindowStyle(WindowStyle.Border);

                this.videoWindow.SetWindowPosition(0, 0, this.panel1.Width, this.panel1.Height);
                hr = this.videoWindow.put_Owner(this.panel1.Handle);
                DsError.ThrowExceptionForHR(hr);
            }
        }
        public void Stop()
        {
            if (this.mediaControl != null)
            {
                this.mediaControl.StopWhenReady();
            }
            if (this.IsDisposed == false)
            {
                this.Parent.Controls.Remove(this);
                base.Dispose(true);
            }
        }
        
    }
}
