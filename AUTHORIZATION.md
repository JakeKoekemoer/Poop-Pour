# Authorization Guide

This project uses custom authorization with MediatR pipeline behaviors, allowing you to secure Queries and Commands declaratively.

## Overview

- **Custom Authorization Attribute**: `[Authorize]` attribute for Queries/Commands
- **Pipeline Behavior**: Automatically checks authorization before executing handlers
- **Roles & Policies**: Support for roles, policies, or both in combination
- **Domain Constants**: `Roles.cs` and `Policies.cs` in Domain layer

## Domain Layer Constants

### Roles (`PoopNPour.Domain/Roles.cs`)

Define application roles as constants:

```csharp
public static class Roles
{
    public const string Administrator = "Administrator";
    public const string User = "User";
    public const string Guest = "Guest";
}
```

### Policies (`PoopNPour.Domain/Policies.cs`)

Define application policies as constants:

```csharp
public static class Policies
{
    public const string RequireAdministrator = "RequireAdministrator";
    public const string RequireUser = "RequireUser";
}
```

## Usage Examples

### 1. Require Specific Role(s)

```csharp
using PoopNPour.Application.Authorization;
using PoopNPour.Domain;

namespace PoopNPour.Application.Products.Queries;

[Authorize(Roles.Administrator)]
public record GetProductsQuery : IRequest<IEnumerable<Product>>;

public class GetProductsQueryHandler 
    : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    // Handler implementation
}
```

### 2. Require Multiple Roles (OR - any role)

```csharp
[Authorize(Roles.Administrator, Roles.Manager)]
public record DeleteProductCommand(int Id) : IRequest;
```

### 3. Require All Roles (AND)

```csharp
[Authorize(Roles = new[] { Roles.Administrator, Roles.Manager }, RequireAllRoles = true)]
public record UpdateProductCommand(int Id, string Name) : IRequest;
```

### 4. Require Policy

```csharp
[Authorize(Policies = new[] { Policies.RequireAdministrator })]
public record CreateProductCommand(string Name) : IRequest<Product>;
```

### 5. Require Multiple Policies (OR - any policy)

```csharp
[Authorize(Policies = new[] { Policies.CanManageProducts, Policies.CanEditProducts })]
public record UpdateProductCommand(int Id, string Name) : IRequest;
```

### 6. Require All Policies (AND)

```csharp
[Authorize(Policies = new[] { Policies.CanManageProducts, Policies.CanEditProducts }, RequireAllPolicies = true)]
public record DeleteProductCommand(int Id) : IRequest;
```

### 7. Combine Roles and Policies

```csharp
// User must have Administrator role AND satisfy CanManageProducts policy
[Authorize(
    Roles = new[] { Roles.Administrator }, 
    Policies = new[] { Policies.CanManageProducts },
    RequireAllRoles = true,
    RequireAllPolicies = true)]
public record CreateProductCommand(string Name) : IRequest<Product>;
```

## Configuring Policies

Policies are configured in `Infrastructure/DependencyInjection.cs`:

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.RequireAdministrator, policy =>
        policy.RequireRole(Roles.Administrator));
    
    options.AddPolicy(Policies.RequireUser, policy =>
        policy.RequireRole(Roles.User));
    
    // Custom policy with multiple roles
    options.AddPolicy(Policies.CanManageProducts, policy =>
        policy.RequireRole(Roles.Administrator, Roles.Manager));
    
    // Custom policy with claims
    options.AddPolicy(Policies.CanViewReports, policy =>
        policy.RequireClaim("Permission", "ViewReports"));
});
```

## How It Works

1. **Pipeline Behavior**: `AuthorizationBehavior<TRequest, TResponse>` intercepts all MediatR requests
2. **Attribute Check**: Checks if the request has an `[Authorize]` attribute
3. **Authentication**: Verifies user is authenticated
4. **Role Check**: Validates user has required role(s)
5. **Policy Check**: Validates user satisfies required policy/policies
6. **Exception**: Throws `UnauthorizedAccessException` if authorization fails
7. **Proceed**: Continues to handler if authorization succeeds

## Authorization Rules

- **No Attribute**: Request proceeds without authorization check
- **Roles Only**: User must have at least one role (OR) or all roles (AND) if `RequireAllRoles = true`
- **Policies Only**: User must satisfy at least one policy (OR) or all policies (AND) if `RequireAllPolicies = true`
- **Roles + Policies**: Both role and policy checks must pass

## Error Handling

When authorization fails, an `UnauthorizedAccessException` is thrown with a descriptive message:

```csharp
throw new UnauthorizedAccessException("User does not have required role(s): Administrator");
```

Handle this in your API layer or use a global exception handler.

## Best Practices

1. ✅ **Use Domain Constants**: Always use `Roles.Administrator` instead of `"Administrator"`
2. ✅ **Define Policies**: Create policies for complex authorization logic
3. ✅ **Combine When Needed**: Use both roles and policies for fine-grained control
4. ✅ **Document Policies**: Add comments explaining what each policy checks
5. ✅ **Test Authorization**: Write tests to verify authorization behavior

## Example: Complete Feature with Authorization

```csharp
// Products/Queries/GetProduct/GetProduct.cs
using MediatR;
using PoopNPour.Application.Authorization;
using PoopNPour.Application.Products.Models;
using PoopNPour.Domain;

namespace PoopNPour.Application.Products.Queries;

[Authorize(Roles.User)] // Any authenticated user can view products
public record GetProductQuery(int Id) : IRequest<Product?>;

public class GetProductQueryHandler 
    : IRequestHandler<GetProductQuery, Product?>
{
    // Implementation
}

// Products/Commands/CreateProduct/CreateProduct.cs
[Authorize(
    Roles = new[] { Roles.Administrator }, 
    Policies = new[] { Policies.CanManageProducts })]
public record CreateProductCommand(string Name, decimal Price) : IRequest<Product>;

public class CreateProductCommandHandler 
    : IRequestHandler<CreateProductCommand, Product>
{
    // Implementation
}
```
