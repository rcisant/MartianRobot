using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using MartianRobot.Api;
using MartianRobot.Api.Infrastructure.Filters;
using MartianRobot.Api.Policies;
using MartianRobot.Domain.Instrumentation.Exceptions;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// HTTP configuration pipeline
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add FluentValidations configuration
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddFluentValidations(this IMvcBuilder builder) =>
            builder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ApiVersioning>());

        /// <summary>
        /// Add FluentValidations configuration
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcCoreBuilder AddFluentValidations(this IMvcCoreBuilder builder) =>
            builder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ApiVersioning>());

        /// <summary>
        /// Add web api protection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddCustomAzureAdProtection(this IServiceCollection services, IConfiguration configuration)
        {
            // Using Microsoft.Identity.Web templates
            // See https://docs.microsoft.com/es-es/azure/active-directory/develop/scenario-protected-web-api-app-configuration
            services.AddMicrosoftIdentityWebApiAuthentication(configuration, "AzureAd");

            // Policy-based authorization in ASP.NET Core
            // See https://docs.microsoft.com/es-es/aspnet/core/security/authorization/policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AdministratorPolicy.Name, AdministratorPolicy.Build);
            });

            return services;
        }

        /// <summary>
        /// Format json response data.
        /// See https://docs.microsoft.com/es-es/aspnet/core/web-api/advanced/formatting
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddCustomJsonOptions(this IMvcBuilder builder) =>
            builder.AddJsonOptions(options =>
            {
                // Use the default property (Pascal) casing.
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

        /// <summary>
        /// Format json response data.
        /// See https://docs.microsoft.com/es-es/aspnet/core/web-api/advanced/formatting
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcCoreBuilder AddCustomJsonOptions(this IMvcCoreBuilder builder) =>
            builder.AddJsonOptions(options =>
            {
                // Use the default property (Pascal) casing.
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

        /// <summary>
        /// Add health checks for external dependencies here.
        /// See https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services) =>
            services
                .AddHealthChecks()
                .Services;

        /// <summary>
        /// Configures Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
            => services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();
                options.SchemaFilter<EnumSchemaFilter>();
                options.ExampleFilters();
                options.IgnoreObsoleteActions();

                options.OperationFilter<ApiVersionOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                var apiDescriptionProvider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var apiVersionDescription in apiDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        apiVersionDescription.GroupName,
                        new OpenApiInfo
                        {
                            Contact = new OpenApiContact { Email = "admin@sgs.com" },
                            Version = apiVersionDescription.ApiVersion.ToString(),
                            Title = $"MartianRobot {apiVersionDescription.ApiVersion}",
                            License = new OpenApiLicense(),
                            Description = apiVersionDescription.IsDeprecated
                                ? "This API version is deprecated"
                                : "Application Service Catalog API"
                        });
                }

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

            }).AddSwaggerExamplesFromAssemblyOf<ApiVersioning>();

        /// <summary>
        /// Configures the Api versioning
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            return services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });
        }

        /// <summary>
        /// Configures the error handling middlewere
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Title = "https://sgs.com/400",
                        Detail = ApiConstants.ModelStateValidation
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes =
                        {
                            "application/problem+json",
                            "application/problem+xml"
                        }
                    };
                };
            });

            return services.AddProblemDetails(config =>
            {

                config.Map<BusinessValidationException>((context, ex) =>
                    new ProblemDetails
                    {
                        Title = BusinessValidationException.DefaultMessage,
                        Status = StatusCodes.Status412PreconditionFailed,
                        Detail = ex.ToString()
                    });

                config.Map<ResourceAlreadyInUseException>((context, ex) =>
                    new ProblemDetails
                    {
                        Title = ResourceAlreadyInUseException.DefaultMessage,
                        Status = StatusCodes.Status403Forbidden,
                        Detail = ex.ToString()
                    });

                config.Map<ResourceAlreadyExistException>((context, ex) =>
                    new ProblemDetails
                    {
                        Title = ResourceAlreadyExistException.DefaultMessage,
                        Status = StatusCodes.Status409Conflict,
                        Detail = ex.ToString()
                    });

                config.Map<ResourceNotFoundException>((context, ex) =>
                    new ProblemDetails
                    {
                        Title = ResourceNotFoundException.DefaultMessage,
                        Status = StatusCodes.Status404NotFound,
                        Detail = ex.ToString()
                    });


                config.Map<Exception>((context, ex) =>
                    new ProblemDetails
                    {
                        Title = "An unhandled error occurred processing your request",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = ex.ToString()
                    });
            });
        }

        /// <summary>
        /// Configures the AutoMapper mapping profiles
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(new string[] { "MartianRobot.Application" });
            });

            services.AddSingleton(sp => autoMapperConfig.CreateMapper());
            

            return services;
        }
    }
}
