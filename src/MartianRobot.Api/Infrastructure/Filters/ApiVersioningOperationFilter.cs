using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace MartianRobot.Api.Infrastructure.Filters
{
    public class ApiVersionOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiVersion = context.ApiDescription.GetApiVersion();
            if (apiVersion is null)
            {
                return;
            }

            var parameters = operation.Parameters;
            if (parameters is null)
            {
                operation.Parameters = parameters = new List<OpenApiParameter>();
            }

            var parameter = parameters.FirstOrDefault(p => p.Name == "api-version");
            if (parameter is null)
            {
                parameter = new OpenApiParameter {
                    Name = "api-version",
                    In = ParameterLocation.Query,
                    Required = true,
                    AllowEmptyValue = false,
                    AllowReserved = false,
                    Example = new OpenApiString(apiVersion.ToString()),
                    Schema = new OpenApiSchema {
                        Type = "string",
                        Default = new OpenApiString(apiVersion.ToString())
                    }
                };
                parameters.Add(parameter);
            }
            parameter.Description = "The requested API version";
        }
    }
}
