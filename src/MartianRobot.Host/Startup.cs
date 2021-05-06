using Hellang.Middleware.ProblemDetails;
using MartianRobot.Api.Infrastructure.Extensions;
using MartianRobot.Application.Services.UriService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.AspNetCore.Hosting;

namespace MartianRobot.Host
{
    /// <summary>
    /// App startup in ASP.NET Core.
    /// See https://docs.microsoft.com/es-es/aspnet/core/fundamentals/startup
    /// </summary>
    public sealed class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures the services to add to the ASP.NET Core Injection of Control (IoC) container. This method gets
        /// called by the ASP.NET runtime.
        /// See https://docs.microsoft.com/es-es/aspnet/core/fundamentals/dependency-injection
        /// </summary>
        /// <param name="services">The services.</param>
        /// //AddDbContext<MartianRobotContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MartianRobot")))
        public void ConfigureServices(IServiceCollection services) =>
            services
                // Enable Application Insights telemetry.
                // See https://docs.microsoft.com/es-es/azure/azure-monitor/app/asp-net-core
                .AddApplicationInsightsTelemetry()
                .AddOpenApi()
                .AddCustomApiVersioning()
                .AddVersionedApiExplorer()
                .AddCustomProblemDetails()
                .AddAutoMapper()
                .ConfigureAutoMapper()
                .AddCustomHealthChecks()
                // Register Services, Repositories and Validators.
                .RegisterApplicationDependencies(Configuration)
                // Add support to protect this API and configure your policies
                .AddCustomAzureAdProtection(Configuration)
                //.AddTransient<MartianRobotContextFactory>()  
                //.AddTransient(provider => provider.GetService<MartianRobotContextFactory>().CreateDbContext(new string[] { Configuration.GetConnectionString("MartianRobot") }))
                //.ConfigureAuthenticationJwt(Configuration)
                //.AddSingleton(typeof(IDesignTimeDbContextFactory<>), typeof(MartianRobotContext<>))
                // Bootstrapping approaches
                // See https://www.strathweb.com/2020/02/asp-net-core-mvc-3-x-addmvc-addmvccore-addcontrollers-and-other-bootstrapping-approaches/
                .AddHttpContextAccessor()
                .AddSingleton<IUriService>(o =>
                {
                    var accessor = o.GetRequiredService<IHttpContextAccessor>();
                    var request = accessor.HttpContext.Request;
                    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                    return new UriService(uri);
                 })
                //.AddSingleton(typeof(IProvider), this.ProviderBehavior)
                //.AddSingleton(typeof(IMonitor), this.MonitoringBehavior)
                .AddControllers().AddNewtonsoftJson(options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddCustomJsonOptions()
                .AddFluentValidations();


        /// <summary>
        /// Configures the application and HTTP request pipeline. Configure is called after ConfigureServices is
        /// called by the ASP.NET runtime.
        /// See https://docs.microsoft.com/es-es/aspnet/core/fundamentals/routing
        /// </summary>
        /// <param name="app">The application builder.</param>
        public void Configure(IApplicationBuilder app) =>
            app
                .UseMiddleware<LoggerMiddleware>()
                .UseSerilogRequestLogging(opts => opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest)
                .UseRouting()
                .UseCors(builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                })
                .UseProblemDetails()
                .UseCustomSerilogRequestLogging()
                .UseMiddleware<ErrorHandlerMiddleware>()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute().RequireAuthorization();
                    // Add basic health check.
                    // See https://docs.microsoft.com/es-es/aspnet/core/host-and-deploy/health-checks
                    endpoints.MapHealthChecks("/health");
                })
                .UseOpenApi();

    }
}
