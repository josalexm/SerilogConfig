using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SECF.Web.Models.Serilog;
using SendGrid;
using Serilog;
using Serilog.Sinks.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SECF.Web
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
            #region Serilog configuration

            LoggerConfiguration serilogConfiguration = new LoggerConfiguration();
            SerilogEmailParamsDto serilogParams = Configuration.GetSection("SerilogEmailParams").Get<SerilogEmailParamsDto>();
            serilogParams.Destinations.ForEach(destination => {
                EmailConnectionInfo emailConnectionInfo = new EmailConnectionInfo
                {
                    EmailSubject = serilogParams.Subject,
                    FromEmail = serilogParams.FromEmail,
                    SendGridClient = new SendGridClient(serilogParams.ApiKeySendGrid),
                    FromName = serilogParams.FromName,
                    ToEmail = destination
                };

                serilogConfiguration.ReadFrom.Configuration(Configuration)
                    .WriteTo.Email(emailConnectionInfo, restrictedToMinimumLevel: serilogParams.LogEventLevel, period: TimeSpan.FromMinutes(5));
            });

            Log.Logger = serilogConfiguration.CreateLogger();
            services.AddLogging(configure => { configure.AddSerilog(); });

            #endregion

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
