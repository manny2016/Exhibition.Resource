

namespace OfficialHelper
{
    using Exhibition.Core;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Text.RegularExpressions;
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
                            var fullName = Path.Combine(Environment.CurrentDirectory, @"assets\userfiles\usr_info.txt");
                            var target = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, @"assets\userfiles\Official"));
                            target.CreateIfNotExists();
                            if (File.Exists(fullName))
                            {
                                var lines = File.ReadAllText(fullName).Split(new char[] { '\r', '\n' }).Where(o => !string.IsNullOrEmpty(o));
                                var index = 0;
                                foreach (var line in lines)
                                {
                                    index++;
                                    if (string.IsNullOrEmpty(line.Trim())) continue;
                                    //Id|Title|Url|Date
                                    try
                                    {
                                        var array = line.Split('|');
                                        //Date-Id
                                        var fileName = new FileInfo(Path.Combine(target.FullName, TryParseFileName(array[array.Length - 3], index)));
                                        if (fileName.Exists) fileName.Delete();
                                        using (var stream = new FileStream(fileName.FullName, FileMode.Create))
                                        {
                                            var writer = new StreamWriter(stream);
                                            writer.Write(array[array.Length - 3]);
                                            writer.Flush();
                                        }
                                        Logger.Info(long.Parse(array[array.Length - 1]).ToDateTimeFromUnixStamp().ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error($"FileName issue {fullName}");
                                        continue;
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
                        Thread.CurrentThread.Join(TimeSpan.FromDays(1));
                    }
                });
            });
            scheduler.Start();
        }

        static string TryParseFileName(string url, int index)
        {
            var fileName = string.Concat("{0}-", index.ToString("00000"),".txt");
            var html = url.GetUriContentDirectly((http) =>
                {
                    http.Method = "GET";

                    return http;
                });
            var pattern = ",s=\"(.+)\";";
            var match = Regex.Match(html, pattern);
            foreach(var g in match.Groups)
            {
               if(TryParseDateTime(g.ToString(),out DateTime? dateTime))
                {
                    fileName = string.Format(fileName, dateTime?.ToString("yyyy-MM-dd"));
                    return fileName;
                }
            }
        
            return fileName;
        }
        static bool TryParseDateTime(string text, out DateTime? date)
        {
            date = null;
            try
            {
                date = DateTime.Parse(text);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
