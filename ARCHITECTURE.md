# Architecture Guidelines

This document establishes the architectural patterns and rules that **MUST** be followed in this project.

## Core Principles

### 1. Clean Architecture

We follow **Clean Architecture** principles, which enforce separation of concerns and dependency inversion.

#### Layer Structure

```
┌─────────────────────────────────────┐
│         API Layer (Presentation)    │  ← Controllers/Endpoints only
├─────────────────────────────────────┤
│      Application Layer (Use Cases)  │  ← Business Logic, CQRS
├─────────────────────────────────────┤
│      Domain Layer (Entities)        │  ← Domain Models (if needed)
├─────────────────────────────────────┤
│    Infrastructure Layer (External)  │  ← Data Access, External Services
└─────────────────────────────────────┘
```

#### Dependency Rule

**Dependencies point inward:**
- API Layer → Application Layer
- Application Layer → Domain Layer
- Infrastructure Layer → Domain Layer + Application Layer

**Never:**
- ❌ Application Layer → API Layer
- ❌ Domain Layer → Any other layer (Domain is the innermost layer)
- ❌ Application Layer → Infrastructure Layer (use interfaces/abstractions)
- ❌ Domain Layer → Infrastructure Layer

### 2. CQRS (Command Query Responsibility Segregation)

We **MUST** use CQRS pattern for all operations. This separates read operations (Queries) from write operations (Commands).

#### Query Pattern (Read Operations)

**Structure:**
```
Application/
  {FeatureName}/              ← Plural form (e.g., Products, Users)
    Queries/
      Get{Entity}/            ← Query folder (without "Query" suffix)
        Get{Entity}.cs        ← Query + Handler in same file
    Models/
      {Entity}.cs             ← Domain model/DTO
```

**Rules:**
- ✅ Queries are **read-only** operations
- ✅ Queries return data, never modify state
- ✅ Query names start with `Get`, `List`, `Find`, `Search`
- ✅ Queries implement `IRequest<TResponse>`
- ✅ Handlers implement `IRequestHandler<TQuery, TResponse>`
- ✅ **Query and Handler MUST be in the same file**

**Example:**
```csharp
// Products/Queries/GetProduct/GetProduct.cs - Query and Handler combined
using MediatR;
using PoopNPour.Application.Products.Models;

namespace PoopNPour.Application.Products.Queries;

public record GetProductQuery : IRequest<IEnumerable<Product>>;

public class GetProductQueryHandler 
    : IRequestHandler<GetProductQuery, IEnumerable<Product>>
{
    public Task<IEnumerable<Product>> Handle(
        GetProductQuery request, 
        CancellationToken cancellationToken)
    {
        // Read-only logic
    }
}
```

#### Command Pattern (Write Operations)

**Structure:**
```
Application/
  {FeatureName}/              ← Plural form (e.g., Products, Users)
    Commands/
      Create{Entity}/         ← Command folder (without "Command" suffix)
        Create{Entity}.cs     ← Command + Handler in same file
        Create{Entity}Validator.cs  ← Validation (optional, separate file)
```

**Rules:**
- ✅ Commands **modify state** (Create, Update, Delete)
- ✅ Commands return minimal data (usually just success/ID)
- ✅ Command names are verbs: `Create`, `Update`, `Delete`, `Archive`
- ✅ Commands implement `IRequest<TResponse>` or `IRequest`
- ✅ Handlers implement `IRequestHandler<TCommand, TResponse>`
- ✅ **Command and Handler MUST be in the same file**

**Example:**
```csharp
// Command
public record CreateProductCommand(
    string Name, 
    decimal Price, 
    string? Description
) : IRequest<Product>;

// Handler
public class CreateProductCommandHandler 
    : IRequestHandler<CreateProductCommand, Product>
{
    public Task<Product> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        // Write logic
    }
}
```

## Project Structure Conventions

### Domain Layer (`PoopNPour.Domain`)

**Required Structure:**
```
PoopNPour.Domain/
  Entities/
    {Entity}.cs                 ← Domain entities (database models)
```

**Rules:**
- ✅ Domain entities represent the core business objects
- ✅ Entities contain only data properties (no business logic)
- ✅ Domain layer has **NO dependencies** on other layers
- ✅ Entities are used by Infrastructure layer for database mapping
- ✅ Application layer works with DTOs, not directly with domain entities

**Example:**
```csharp
namespace PoopNPour.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

### Application Layer (`PoopNPour.Application`)

**Required Structure:**
```
PoopNPour.Application/
  {FeatureName}/              ← Plural form (e.g., Products, Users)
    Queries/
      Get{Entity}/            ← Query folder (without "Query" suffix)
        Get{Entity}.cs        ← Query + Handler in same file
    Commands/
      Create{Entity}/         ← Command folder (without "Command" suffix)
        Create{Entity}.cs     ← Command + Handler in same file
    Models/
      {Entity}.cs             ← DTOs (Data Transfer Objects)
    Repositories/
      I{Entity}Repository.cs ← Repository interfaces
    DTOs/                     ← Optional: Response DTOs if different from models
      {Entity}Dto.cs
```

**Rules:**
- ✅ Feature folders use **plural** names (e.g., `Products`, `Users`, `Orders`)
- ✅ One feature per folder
- ✅ All business logic lives in handlers
- ✅ Models are DTOs (Data Transfer Objects), not domain entities
- ✅ Repository interfaces are defined here (implemented in Infrastructure)
- ✅ No direct database access (use repositories/interfaces)
- ✅ **Query/Command and Handler MUST be in the same file**
- ✅ Application layer references Domain layer for entity types (if needed)

### API Layer (`PoopNPour.Api`)

**Required Structure:**
```
PoopNPour.Api/
  Endpoints/
    {FeatureName}Endpoints.cs   ← All endpoints for a feature (plural, matches Application layer)
  Program.cs                     ← Only configuration and wiring
```

**Rules:**
- ✅ One endpoint file per feature: `{FeatureName}Endpoints.cs` (plural, matches Application layer)
- ✅ Endpoint class name matches Application layer feature name (e.g., `Products` → `ProductsEndpoints`)
- ✅ Endpoint route matches Application layer feature name (e.g., `/weatherforecasts`)
- ✅ Endpoints are **thin** - they only:
  - Receive HTTP requests
  - Create queries/commands
  - Send to MediatR
  - Return HTTP responses
- ✅ **NO business logic** in endpoints
- ✅ Use extension methods: `Map{FeatureName}Endpoints()` (plural)
- ✅ Group endpoints: `app.MapGroup("/{feature}")` (plural, lowercase)
- ✅ Use `Produces<T>()` for OpenAPI documentation

**Example:**
```csharp
// Application layer: Products/
// API layer: Endpoints/ProductsEndpoints.cs
public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products")  // Plural, matches Application layer
            .WithTags("Products");

        group.MapGet("/", GetProducts)
            .WithName("GetProducts")
            .Produces<IEnumerable<Product>>();
    }

    private static async Task<IResult> GetProducts(IMediator mediator)
    {
        var query = new GetProductsQuery();
        var products = await mediator.Send(query);
        return Results.Ok(products);
    }
}
```

## Naming Conventions

### Files and Folders
- ✅ Feature folders: **Plural** (e.g., `Products`, `Users`)
- ✅ Query folders: `Get{Entity}/` (without "Query" suffix)
- ✅ Query files: `Get{Entity}.cs` (contains Query + Handler, inside query folder)
- ✅ Command folders: `Create{Entity}/` (without "Command" suffix)
- ✅ Command files: `Create{Entity}.cs` (contains Command + Handler, inside command folder)
- ✅ Endpoint files: `{FeatureName}Endpoints.cs` (plural, matches Application layer feature name)

### Classes and Records
- ✅ Queries: `Get{Entity}Query`, `List{Entity}Query`
- ✅ Commands: `Create{Entity}Command`, `Update{Entity}Command`
- ✅ Handlers: `{Query/Command}Handler`
- ✅ Endpoints: `{FeatureName}Endpoints`

## MediatR Configuration

**Required Setup:**
```csharp
// In Program.cs
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(GetProductsQuery).Assembly));
```

**Rules:**
- ✅ Register MediatR in `Program.cs`
- ✅ Register handlers from Application assembly
- ✅ Use a query/command type from Application layer for assembly reference

## What NOT to Do

### ❌ Anti-Patterns to Avoid

1. **Business Logic in API Layer**
   ```csharp
   // ❌ BAD
   app.MapGet("/products", () => {
       var products = new[] { "Product1", "Product2" };
       // Business logic here
   });
   
   // ✅ GOOD
   app.MapGet("/products", async (IMediator mediator) => {
       var query = new GetProductsQuery();
       return await mediator.Send(query);
   });
   ```

2. **Direct Database Access in Handlers**
   ```csharp
   // ❌ BAD - Direct DbContext access
   public class GetProductsQueryHandler {
       private readonly AppDbContext _context; // ❌
   }
   
   // ✅ GOOD - Use repository interface
   public class GetProductsQueryHandler {
       private readonly IProductRepository _repository; // ✅
   }
   ```

3. **Skipping CQRS**
   ```csharp
   // ❌ BAD - Direct service call
   app.MapGet("/products", (IProductService service) => {
       return service.GetProducts();
   });
   
   // ✅ GOOD - Use MediatR query
   app.MapGet("/products", async (IMediator mediator) => {
       return await mediator.Send(new GetProductsQuery());
   });
   ```

4. **Mixing Read and Write Operations**
   ```csharp
   // ❌ BAD - Query that modifies state
   public class GetProductsQueryHandler {
       public Task Handle(...) {
           _context.Logs.Add(...); // ❌ Modifying in query
       }
   }
   ```

## Checklist for New Features

When adding a new feature, ensure:

- [ ] Feature folder created in `Application/{FeatureName}/`
- [ ] Query/Command created with proper naming
- [ ] Handler implements `IRequestHandler<TRequest, TResponse>`
- [ ] Endpoint file created: `Api/Endpoints/{FeatureName}Endpoints.cs`
- [ ] Endpoint registered in `Program.cs`
- [ ] No business logic in API layer
- [ ] MediatR used for all operations
- [ ] Proper namespaces and using statements
- [ ] OpenAPI documentation added (`Produces<T>()`)

## References

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [MediatR Documentation](https://github.com/jbogard/MediatR)

---

**Remember:** These are not suggestions - they are **mandatory rules** for maintaining consistency and quality in this codebase.
