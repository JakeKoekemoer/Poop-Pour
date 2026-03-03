using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using PoopNPour.Application.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PoopNPour.Api.Common;

/// <summary>
/// Automatically infers response types and status codes using reflection
/// Eliminates the need for manual .Produces() calls on each endpoint
/// </summary>
public class AutoResponseTypesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var methodInfo = context.MethodInfo;
        if (methodInfo == null) return;

        // Infer response type from handler method
        var responseType = InferResponseType(methodInfo, context);
        
        // Always add 200 OK for successful responses (or 201 for POST)
        var httpMethod = context.ApiDescription.HttpMethod?.ToUpperInvariant();
        if (httpMethod == "POST" && responseType != null)
        {
            AddResponse(operation, context, 201, "Created", responseType);
        }
        else if (responseType != null)
        {
            AddResponse(operation, context, 200, "Success", responseType);
        }

        var authorizationRequired = CheckAuthorizationRequirements(methodInfo, out bool hasRolesOrPolicies);
        
        if (authorizationRequired)
        {
            AddResponse(operation, context, 401, "Unauthorized", null);
            
            if (hasRolesOrPolicies)
            {
                AddResponse(operation, context, 403, "Forbidden", null);
            }
        }

        if (httpMethod == "GET" && HasRouteParameters(context))
        {
            AddResponse(operation, context, 404, "Not Found", null);
        }

        if (httpMethod is "POST" or "PUT" or "PATCH")
        {
            AddResponse(operation, context, 400, "Bad Request", null);
        }
    }

    private Type? InferResponseType(MethodInfo methodInfo, OperationFilterContext context)
    {
        var returnType = methodInfo.ReturnType;

        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            returnType = returnType.GetGenericArguments()[0];
        }

        // IResult doesn't have a specific type, so we need to inspect the method body
        // or parameters to find the MediatR request
        var parameters = methodInfo.GetParameters();
        var mediatorParam = parameters.FirstOrDefault(p => p.ParameterType.Name == "IMediator");
        
        if (mediatorParam != null)
        {
            // Look for the MediatR request type in the method parameters or body
            // Find parameters with [FromBody], [FromQuery], [FromRoute], or records/DTOs
            var requestParam = parameters.FirstOrDefault(p => 
                p.GetCustomAttribute<FromBodyAttribute>() != null ||
                p.GetCustomAttribute<FromQueryAttribute>() != null ||
                p.ParameterType.IsClass && 
                p.ParameterType != typeof(string) && 
                !p.ParameterType.IsPrimitive &&
                p.ParameterType.Namespace?.StartsWith("PoopNPour") == true);

            if (requestParam != null)
            {
                // Try to find corresponding command/query
                var commandOrQueryType = FindMediatRRequestType(methodInfo);
                if (commandOrQueryType != null)
                {
                    return GetMediatRResponseType(commandOrQueryType);
                }
            }
        }

        return null;
    }

    private Type? FindMediatRRequestType(MethodInfo methodInfo)
    {
        var declaringType = methodInfo.DeclaringType;
        if (declaringType == null) return null;

        var methodName = methodInfo.Name;
        
        // Try to find command/query by convention
        // e.g., GetUsersAsync -> GetUsersQuery
        // e.g., CreateUserAsync -> CreateUserCommand
        
        var potentialNames = new[]
        {
            methodName.Replace("Async", "Query"),
            methodName.Replace("Async", "Command"),
            methodName.Replace("Async", "").Replace("Get", "Get") + "Query",
            methodName.Replace("Async", "").Replace("Create", "Create") + "Command",
            methodName.Replace("Async", "").Replace("Update", "Update") + "Command",
            methodName.Replace("Async", "").Replace("Delete", "Delete") + "Command"
        };

        // Search in Application assembly
        var applicationAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name?.Contains("Application") == true);

        if (applicationAssembly != null)
        {
            foreach (var name in potentialNames)
            {
                var type = applicationAssembly.GetTypes()
                    .FirstOrDefault(t => t.Name == name);
                
                if (type != null) return type;
            }
        }

        return null;
    }

    private Type? GetMediatRResponseType(Type requestType)
    {
        // Find IRequest<TResponse> interface
        var requestInterface = requestType.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && 
                                 i.GetGenericTypeDefinition().Name.Contains("IRequest"));

        if (requestInterface != null && requestInterface.IsGenericType)
        {
            var genericArgs = requestInterface.GetGenericArguments();
            if (genericArgs.Length > 0)
            {
                return genericArgs[0];
            }
        }

        return null;
    }

    private bool CheckAuthorizationRequirements(MethodInfo methodInfo, out bool hasRolesOrPolicies)
    {
        hasRolesOrPolicies = false;

        var requestType = FindMediatRRequestType(methodInfo);
        if (requestType == null) return false;

        var authorizeAttr = requestType.GetCustomAttribute<AuthorizeAttribute>();
        if (authorizeAttr == null) return false;

        var roles = authorizeAttr.GetRoles();
        var policies = authorizeAttr.GetPolicies();
        
        hasRolesOrPolicies = (roles?.Any() ?? false) || (policies?.Any() ?? false);

        return true;
    }

    private bool HasRouteParameters(OperationFilterContext context)
    {
        return context.ApiDescription.ParameterDescriptions
            .Any(p => p.Source.Id == "Path");
    }

    private void AddResponse(OpenApiOperation operation, OperationFilterContext context, int statusCode, string description, Type? responseType)
    {
        operation.Responses ??= new OpenApiResponses();

        var statusCodeString = statusCode.ToString();
        
        if (!operation.Responses.ContainsKey(statusCodeString))
        {
            var response = new OpenApiResponse
            {
                Description = description
            };

            if (responseType != null)
            {
                response.Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(responseType, context.SchemaRepository)
                    }
                };
            }

            operation.Responses.Add(statusCodeString, response);
        }
    }
}
