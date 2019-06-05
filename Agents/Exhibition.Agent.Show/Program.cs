

namespace Exhibition.Agent.Show
{
    using Chromium;
    using Chromium.WebBrowser;
    using System;
    using System.IO;
    using System.Windows.Forms;
    using Exhibition.Core.Models;
    using Exhibition.Core;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var json = CreateDirective().SerializeToJson();

            Application.ThreadExit += Application_ThreadExit;
            string assemblyDir = Path.GetDirectoryName(
              new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath
            );
            CfxRuntime.LibCefDirPath = assemblyDir;
            CfxRuntime.LibCfxDirPath = CfxRuntime.LibCefDirPath;
            ChromiumWebBrowser.OnBeforeCfxInitialize += (e) =>
            {
                e.Settings.CachePath = Path.Combine(assemblyDir, "cache");
                e.Settings.ResourcesDirPath = Path.Combine(assemblyDir, "Resources");
                e.Settings.LocalesDirPath = Path.Combine(e.Settings.ResourcesDirPath, "locales");
            };
            ChromiumWebBrowser.OnBeforeCommandLineProcessing += (e) =>
            {
                // add command line switch
            };

            ChromiumWebBrowser.Initialize();

            AgentHost.HostOperationSerivceViaConfiguration();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ForumMain());
        }
        private static void Application_ThreadExit(object sender, EventArgs e)
        {
            CfxRuntime.Shutdown();
        }

        public static Directive CreateDirective()
        {
            return new Directive()
            {
                DefaultWindow = new Window() { Id = 1, Location = new WinPoint() { X = 0, Y = 0 }, Size = new WinSize() { Width = 1024, Height = 768 } },
                Description = "播放视频",
                Name = "播放宣传片",
                Resources = new Resource[] { new Resource() { Name = "1.avi", Workspace = "videos", Type = ResourceTypes.Video } },
                Type = DirectiveTypes.Run,
                Terminal = new MediaPlayerTerminal()
                {
                    Description = "视频播放器",
                    Name = "大屏",
                    Settings = new MedaiPlayerSettings()
                    {
                        Endpoint = "http://192.168.0.102/api/common/run",
                        Ip = "192.168.0.102",
                        Windows = new Window[] {
                         new Window(){  Id =1, Location = new WinPoint(){ X=0,Y=0 } , Size= new WinSize(){  Width=1024, Height = 768} },
                         new Window(){  Id =2, Location = new WinPoint(){ X=1024,Y=0 } , Size= new WinSize(){  Width=1024, Height = 768} },
                         new Window(){  Id =2, Location = new WinPoint(){ X=2048,Y=0 } , Size= new WinSize(){  Width=1024, Height = 768} },
                    }
                    }
                }

            };
        }
    }
}
