﻿



namespace Exhibition.Portal.Api
{
    using Exhibition.Core;
    using Exhibition.Core.Services;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using System.IO;
    public class Program
    {
        public static void Main(string[] args)
        {            
            CreateWebHostBuilder(args)
                .Build()                
                .Run();
            
        }

        

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
