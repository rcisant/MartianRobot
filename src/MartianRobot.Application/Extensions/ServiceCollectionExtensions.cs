using FluentValidation;
using MartianRobot.Application.Services.MartianRobot;
using MartianRobot.Domain.Entities;
using Microsoft.Extensions.Configuration;
#if memoryCache
using SGS.Framework.Caching.Extensions;
#endif

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
#if memoryCache
            // Memory Cache service
            services.AddSgsMemoryCache(configuration);
#endif


            // Services
       
            services.AddTransient<IMartianRobotService, MartianRobotService>();



            // Validators
       
            services.AddSingleton<IValidator<MartianRobotUpdateable>, MartianRobotUpdateableValidator>();

       
            services.AddSingleton<IValidator<MartianRobotDTO>, MartianRobotValidator>();
            

            return services;
        }
    }
}
