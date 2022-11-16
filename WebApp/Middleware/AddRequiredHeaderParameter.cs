using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace WebApp.Middleware
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation?.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();
            if (context != null && !context.ApiDescription.RelativePath.Contains("login") && !context.ApiDescription.RelativePath.Contains("token/refresh"))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-Auth",
                    Style = ParameterStyle.Label,
                    In = ParameterLocation.Header,
                    Description = "Jwt Bearer Token",
                    Schema = new OpenApiSchema { Type = "string", Format = "Bearer " },
                    AllowEmptyValue = false,
                    Required = true // set to false if this is optional
                });
            }
        }
    }
}
