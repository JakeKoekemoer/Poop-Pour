using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using PoopNPour.Application.Authentication.Models;
using PoopNPour.Application.Users.Models;

namespace PoopNPour.Api.Common;

/// <summary>
/// Adds clear, user-friendly examples to request DTOs in Swagger UI.
/// Replaces generic "string" placeholders with realistic sample values.
/// </summary>
public class SchemaExampleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // if (context.Type == typeof(LoginRequestDto))
        // {
        //     schema.Example = new OpenApiObject
        //     {
        //         ["username"] = new OpenApiString("jane@example.com"),
        //         ["password"] = new OpenApiString("••••••••")
        //     };
        //     return;
        // }
    }
}
