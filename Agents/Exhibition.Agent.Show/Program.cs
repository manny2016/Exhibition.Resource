

namespace Exhibition.Agent.Show
{
    using Chromium;
    using Chromium.WebBrowser;
    using System;
    using System.IO;
    using System.Windows.Forms;
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
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
    }
}
