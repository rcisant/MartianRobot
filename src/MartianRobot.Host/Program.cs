using Microsoft.AspNetCore.Hosting;
using Serilog;
using MartianRobot.Host;
using WebHost = Microsoft.Extensions.Hosting.Host;
using Microsoft.Extensions.Configuration;
using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.EventHubs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public sealed class Program
    {
        private Program() {}

        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            webHost.Run();
        }
        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                })
                // Configure Serilog inline
                // See https://github.com/serilog/serilog-aspnetcore#inline-initialization
                .UseSerilog((hostingContext, services, loggerConfiguration) =>
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                ).ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddCommandLine(args);
                    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                });
    }
}
