

namespace Exhibition.Core
{
    using System;
    using System.IO;
    using System.Threading;
    public class Scheduler
    {
        protected Action<CancellationToken> Action;
        protected bool IsRunning;
        protected bool IsShutdowning;
        protected CancellationTokenSource Cancellation;
        protected AutoResetEvent ShutdownSign;

        public event EventHandler Shutdown;

        public Scheduler(Action<CancellationToken> action)
        {
            this.Action = action;
            this.IsRunning = false;
            this.IsShutdowning = false;
        }

        public void Start()
        {
            if (this.IsRunning == true)
            {
                return;
            }

            var signAzureShutdown = Environment.GetEnvironmentVariable("WEBJOBS_SHUTDOWN_FILE") ?? "C:/test";
            var watcher = new FileSystemWatcher(
                Path.GetDirectoryName(signAzureShutdown),
                Path.GetFileName(signAzureShutdown))
            {
                IncludeSubdirectories = false,
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite,
            };
            watcher.Created += Watcher_Changed;
            watcher.Changed += Watcher_Changed;

            if (this.Action != null)
            {
                this.IsRunning = true;
                try
                {
                    this.Cancellation = new CancellationTokenSource();
                    this.Action(this.Cancellation.Token);
                    if (this.ShutdownSign != null)
                    {
                        this.ShutdownSign.Set();
                    }
                }
                finally
                {
                    this.IsRunning = false;
                }
            }
        }

        public void Abort()
        {
            this.OnAbort(this, null);
        }

        protected void OnAbort(object sender, EventArgs e)
        {
            if (this.IsRunning)
            {
                if (this.IsShutdowning == false)
                {
                    this.IsShutdowning = true;
                    try
                    {
                        this.ShutdownSign = new AutoResetEvent(false);
                        this.Cancellation.Cancel();
                        this.OnShutdown(sender, e);
                        this.ShutdownSign.WaitOne();
                    }
                    finally
                    {
                        this.IsShutdowning = false;
                    }
                }
            }
        }

        protected virtual void OnShutdown(object sender, EventArgs e)
        {
            this.Shutdown?.Invoke(sender, e);
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            this.OnAbort(sender, e);
        }
    }
}
