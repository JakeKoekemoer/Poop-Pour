# Policy-to-Role Mappings Guide

This document explains how to configure authorization policies and their role mappings in the Infrastructure layer.

## Overview

The `PolicyRoleMappings.cs` file in the Infrastructure layer provides a centralized location to configure which roles satisfy which authorization policies. This makes it easy to maintain and understand the authorization structure of your application.

## File Location

`src/PoopNPour.Infrastructure/Authorization/PolicyRoleMappings.cs`

## How It Works

The `PolicyRoleMappings.ConfigurePolicies()` method is called during application startup in `DependencyInjection.cs` to configure all authorization policies with their required roles.

## Adding a New Policy-to-Role Mapping

### Step 1: Define the Policy Constant

Add your policy constant to `src/PoopNPour.Domain/Common/Auth/Policies.cs`:

```csharp
public static class Policies
{
    // ... existing policies ...
    
    public const string CanManageProducts = nameof(CanManageProducts);
    public const string CanViewReports = nameof(CanViewReports);
}
```

### Step 2: Add Policy-to-Role Mapping

Simply add your policy and roles to the `PolicyToRolesMap` dictionary in `PolicyRoleMappings.cs`:

```csharp
private static readonly Dictionary<string, string[]?> PolicyToRolesMap = new()
{
    // ... existing mappings ...
    
    // Single role requirement
    { Policies.CanManageProducts, new[] { Roles.Administrator } },
    
    // Multiple roles (OR - user needs any of these roles)
    { Policies.CanViewReports, new[] { Roles.Administrator, Roles.FamilyHead, Roles.FamilyMember } },
    
    // Authenticated users only (no specific role required)
    { Policies.AuthenticatedOnly, null },
    
    // Public policy (allows anonymous)
    { Policies.PublicEndpoint, Array.Empty<string>() },
};
```

That's it! The `ConfigurePolicies` method automatically configures all policies based on this dictionary.

## Policy Configuration Options

The `PolicyToRolesMap` dictionary supports three types of policy configurations:

### 1. Require Specific Role(s)

```csharp
// Single role
{ Policies.CanManageProducts, new[] { Roles.Administrator } }

// Multiple roles (OR - user needs any of these roles)
{ Policies.CanViewReports, new[] { Roles.Administrator, Roles.FamilyHead, Roles.FamilyMember } }
```

### 2. Require Authenticated User (No Specific Role)

```csharp
// null = any authenticated user can access
{ Policies.AuthenticatedOnly, null }
```

### 3. Public Policy (Allows Anonymous)

```csharp
// Empty array = public, allows anonymous access
{ Policies.PublicEndpoint, Array.Empty<string>() }
```

### Summary

- **Array with roles**: Requires any of the specified roles (OR logic)
- **`null`**: Requires authenticated user only (no specific role)
- **Empty array**: Public policy (allows anonymous)

## Current Policy Mappings

### Account Policies

- **AccountRegistration**: Requires authenticated user (can be changed to `AllowAnonymous()` if registration should be public)
- **AccountForgotPassword**: Requires authenticated user (can be changed to `AllowAnonymous()` if password reset should be public)
- **AccountLogin**: Public (allows anonymous access)

## Usage in Application Layer

Once policies are configured, you can use them in your MediatR requests:

```csharp
using PoopNPour.Application.Authorization;
using PoopNPour.Domain;

[Authorize(Policies = new[] { Policies.CanManageProducts })]
public record CreateProductCommand(string Name) : IRequest<Product>;
```

## Best Practices

1. ✅ **Centralize Configuration**: Always add policy-to-role mappings in `PolicyRoleMappings.cs`
2. ✅ **Use Constants**: Always use constants from `Policies` and `Roles` classes
3. ✅ **Document Complex Policies**: Add comments explaining complex policy requirements
4. ✅ **Keep Helper Methods Updated**: Update `GetRolesForPolicy` and `GetPoliciesForRole` for documentation
5. ✅ **Test Policies**: Write tests to verify policy behavior

## Example: Complete Policy Setup

```csharp
// 1. Add policy constant to Policies.cs
public const string CanManageProducts = nameof(CanManageProducts);

// 2. Add to PolicyToRolesMap dictionary in PolicyRoleMappings.cs
{ Policies.CanManageProducts, new[] { Roles.Administrator, Roles.FamilyHead } }

// 3. Use in Application layer
[Authorize(Policies = new[] { Policies.CanManageProducts })]
public record CreateProductCommand(string Name) : IRequest<Product>;
```

The policy is automatically configured during application startup - no additional code needed!

## Troubleshooting

### Policy Not Working

- Verify the policy is configured in `PolicyRoleMappings.ConfigurePolicies()`
- Check that the policy name matches exactly (case-sensitive)
- Ensure the user has the required role(s)
- Check application logs for authorization failures

### Policy Not Found Error

- Ensure the policy constant exists in `Policies.cs`
- Verify the policy is configured in `ConfigurePolicies()` method
- Check for typos in policy names
