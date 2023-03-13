using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EcommerceApi.Swagger
{
    public class AddHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            var parameter = new OpenApiParameter
            {
                Name = "x-user-id",
                In = ParameterLocation.Header,
                Description = "User ID header for the request",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            };


            operation.Parameters.Add(parameter);
        }
    }
}
