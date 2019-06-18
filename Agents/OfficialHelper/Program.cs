

namespace OfficialHelper
{
    using Exhibition.Core;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            Host.ConfigureServiceProvider((configure) => { });
            var scheduler = new Scheduler((cancellation) =>
            {

                var tasks = new string[] { "Parsing wechat official list." };
                Parallel.ForEach(tasks, (task) =>
                {
                    while (cancellation.IsCancellationRequested == false)
                    {
                        try
                        {
                            Logger.Info($"Start {task}.");
                            var fullName = Path.Combine(Environment.CurrentDirectory, @"assets\userfiles\success.txt");
                            var target = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, @"assets\userfiles\Official"));
                            target.CreateIfNotExists();
                            if (File.Exists(fullName))
                            {
                                var lines = File.ReadAllText(fullName).Split(new char[] { '\r', '\n' });
                                foreach (var line in lines)
                                {
                                    if (string.IsNullOrEmpty(line.Trim())) continue;
                                    //  Id|Title|Url|Date
                                    var array = line.Split('|');
                                    //Date-Id
                                    var fileName = new FileInfo(Path.Combine(target.FullName, $"{array[3]}-{array[0]}.txt"));
                                    if (fileName.Exists) fileName.Delete();
                                    using (var stream = new FileStream(fileName.FullName, FileMode.Create))
                                    {
                                        var writer = new StreamWriter(stream);
                                        writer.Write(array[2]);
                                        writer.Flush();
                                    }
                                }
                            }
                            else
                            {
                                Logger.Warn($"List file cant be find;fullName:{fullName}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }
                        Thread.CurrentThread.Join(TimeSpan.FromHours(1));
                    }
                });
            });
            scheduler.Start();
        }
    }
}
