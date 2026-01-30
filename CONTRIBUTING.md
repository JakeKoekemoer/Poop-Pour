# Contributing Guidelines

## Before You Start

1. **Read [ARCHITECTURE.md](./ARCHITECTURE.md)** - This is mandatory
2. Understand Clean Architecture and CQRS patterns
3. Follow the established project structure

## Adding a New Feature

### Step 1: Create Application Layer Structure

Create the feature folder structure directly in `PoopNPour.Application/`:

```
{FeatureName}/              ← Use plural (e.g., Products, Users)
  Queries/
    Get{Entity}/            ← Query folder (without "Query" suffix)
      Get{Entity}.cs        ← Query + Handler in same file
  Commands/                 ← If write operations needed
    Create{Entity}/         ← Command folder (without "Command" suffix)
      Create{Entity}.cs     ← Command + Handler in same file
  Models/
    {Entity}.cs
```

### Step 2: Implement Query/Command

**For Queries (Read Operations):**
```csharp
// Application/{Feature}/Queries/Get{Entity}/Get{Entity}.cs
// Query and Handler MUST be in the same file
using MediatR;
using PoopNPour.Application.{Feature}.Models;

namespace PoopNPour.Application.{Feature}.Queries;

public record Get{Entity}Query : IRequest<IEnumerable<{Entity}>>;

public class Get{Entity}QueryHandler 
    : IRequestHandler<Get{Entity}Query, IEnumerable<{Entity}>>
{
    public Task<IEnumerable<{Entity}>> Handle(
        Get{Entity}Query request, 
        CancellationToken cancellationToken)
    {
        // Your business logic here
        return Task.FromResult<IEnumerable<{Entity}>>(results);
    }
}
```

**For Commands (Write Operations):**
```csharp
// Application/{Feature}/Commands/Create{Entity}/Create{Entity}.cs
// Command and Handler MUST be in the same file
using MediatR;
using PoopNPour.Application.{Feature}.Models;

namespace PoopNPour.Application.{Feature}.Commands;

public record Create{Entity}Command(
    // Properties here
) : IRequest<{Entity}>;

public class Create{Entity}CommandHandler 
    : IRequestHandler<Create{Entity}Command, {Entity}>
{
    public Task<{Entity}> Handle(
        Create{Entity}Command request, 
        CancellationToken cancellationToken)
    {
        // Your business logic here
        return Task.FromResult(result);
    }
}
```

### Step 3: Create API Endpoint

Create `src/PoopNPour.Api/Endpoints/{FeatureName}Endpoints.cs`:
- **Endpoint name MUST match Application layer feature name** (plural)
- **Route MUST match Application layer feature name** (plural, lowercase)

```csharp
// If Application layer feature is "Products", endpoint is "ProductsEndpoints"
using MediatR;
using PoopNPour.Application.{Feature}.Models;
using PoopNPour.Application.{Feature}.Queries;

namespace PoopNPour.Api.Endpoints;

public static class {FeatureName}Endpoints  // Plural, matches Application layer
{
    public static void Map{FeatureName}Endpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/{feature}")  // Plural, lowercase, matches Application layer
            .WithTags("{FeatureName}");  // Plural, matches Application layer

        group.MapGet("/", Get{Entity})
            .WithName("Get{Entity}")
            .Produces<IEnumerable<{Entity}>>();
    }

    private static async Task<IResult> Get{Entity}(IMediator mediator)
    {
        var query = new Get{Entity}Query();
        var result = await mediator.Send(query);
        return Results.Ok(result);
    }
}
```

### Step 4: Register Endpoint

In `src/PoopNPour.Api/Program.cs`, add:

```csharp
app.Map{FeatureName}Endpoints();
```

## Code Review Checklist

Before submitting, ensure:

- [ ] Feature follows folder structure in `Application/{FeatureName}/` (plural)
- [ ] Query/Command properly implements `IRequest<TResponse>`
- [ ] Handler implements `IRequestHandler<TRequest, TResponse>`
- [ ] **Query/Command and Handler are in the same file**
- [ ] Endpoint file created: `{FeatureName}Endpoints.cs` (plural, matches Application layer)
- [ ] Endpoint class name matches Application layer feature name
- [ ] Endpoint route matches Application layer feature name (plural, lowercase)
- [ ] Endpoint registered in `Program.cs`
- [ ] No business logic in API layer
- [ ] MediatR used for all operations
- [ ] Proper namespaces (plural for features)
- [ ] OpenAPI documentation added (`Produces<T>()`)
- [ ] Code compiles without errors
- [ ] No linter warnings

## Common Mistakes to Avoid

1. ❌ **Putting business logic in endpoints**
2. ❌ **Using singular names for feature folders** (should be plural)
3. ❌ **Skipping CQRS** (direct service calls)
4. ❌ **Mixing read and write operations** in queries
5. ❌ **Creating endpoints without corresponding queries/commands**
6. ❌ **Separating Query/Command and Handler into different files** (must be combined)
7. ❌ **Endpoint naming doesn't match Application layer** (must match exactly)

## Questions?

Refer to [ARCHITECTURE.md](./ARCHITECTURE.md) for detailed explanations and examples.
