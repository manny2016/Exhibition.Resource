using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Exhibition.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Exhibition.Portal.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Access-Control-Allow-Origin", builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                );
            });
            services.AddManagementService();
            services.AddLogging((cfg) =>
            {
                cfg.AddConsole();
                cfg.AddLog4Net();
            });
            services.AddMemoryCache();
            services.AddManagementService();
            services.AddSerialPortListener();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseCors("Access-Control-Allow-Origin");
            app.UseCors("Access-Control-Allow-Methods");
            app.UseCors("Access-Control-Allow-Headers");
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "assets")),
                RequestPath = "/assets"
            });
            
            app.UseMvc();

        }
    }
}
