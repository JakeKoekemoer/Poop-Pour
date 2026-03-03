using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi;

namespace PoopNPour.Api.Common;

/// <summary>
/// Adds clear, user-friendly examples to request DTOs in Swagger UI.
/// Replaces generic "string" placeholders with realistic sample values.
/// </summary>
public class SchemaExampleFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
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
