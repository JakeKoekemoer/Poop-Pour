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
- Application Layer → Domain Layer (if exists)
- Infrastructure Layer → Application Layer

**Never:**
- ❌ Application Layer → API Layer
- ❌ Domain Layer → Application Layer
- ❌ Application Layer → Infrastructure Layer (use interfaces/abstractions)

### 2. CQRS (Command Query Responsibility Segregation)

We **MUST** use CQRS pattern for all operations. This separates read operations (Queries) from write operations (Commands).

#### Query Pattern (Read Operations)

**Structure:**
```
Application/
  Features/
    {FeatureName}/              ← Plural form (e.g., WeatherForecasts, Users)
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
// Queries/GetWeatherForecast/GetWeatherForecast.cs - Query and Handler combined
using MediatR;
using PoopNPour.Application.WeatherForecasts.Models;

namespace PoopNPour.Application.WeatherForecasts.Queries;

public record GetWeatherForecastQuery : IRequest<IEnumerable<WeatherForecast>>;

public class GetWeatherForecastQueryHandler 
    : IRequestHandler<GetWeatherForecastQuery, IEnumerable<WeatherForecast>>
{
    public Task<IEnumerable<WeatherForecast>> Handle(
        GetWeatherForecastQuery request, 
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
  Features/
    {FeatureName}/              ← Plural form (e.g., WeatherForecasts, Users)
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
public record CreateWeatherForecastCommand(
    DateOnly Date, 
    int TemperatureC, 
    string? Summary
) : IRequest<WeatherForecast>;

// Handler
public class CreateWeatherForecastCommandHandler 
    : IRequestHandler<CreateWeatherForecastCommand, WeatherForecast>
{
    public Task<WeatherForecast> Handle(
        CreateWeatherForecastCommand request, 
        CancellationToken cancellationToken)
    {
        // Write logic
    }
}
```

## Project Structure Conventions

### Application Layer (`PoopNPour.Application`)

**Required Structure:**
```
PoopNPour.Application/
  Features/
    {FeatureName}/              ← Plural form (e.g., WeatherForecasts, Users)
      Queries/
        Get{Entity}/            ← Query folder (without "Query" suffix)
          Get{Entity}.cs        ← Query + Handler in same file
      Commands/
        Create{Entity}/         ← Command folder (without "Command" suffix)
          Create{Entity}.cs     ← Command + Handler in same file
      Models/
        {Entity}.cs             ← Domain models/DTOs
      DTOs/                     ← Optional: Response DTOs if different from models
        {Entity}Dto.cs
```

**Rules:**
- ✅ Feature folders use **plural** names (e.g., `WeatherForecasts`, `Users`, `Products`)
- ✅ One feature per folder
- ✅ All business logic lives in handlers
- ✅ Models/DTOs are in the `Models` folder
- ✅ No direct database access (use repositories/interfaces)
- ✅ **Query/Command and Handler MUST be in the same file**

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
- ✅ Endpoint class name matches Application layer feature name (e.g., `WeatherForecasts` → `WeatherForecastsEndpoints`)
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
// Application layer: Features/WeatherForecasts/
// API layer: Endpoints/WeatherForecastsEndpoints.cs
public static class WeatherForecastsEndpoints
{
    public static void MapWeatherForecastsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weatherforecasts")  // Plural, matches Application layer
            .WithTags("WeatherForecasts");

        group.MapGet("/", GetWeatherForecast)
            .WithName("GetWeatherForecast")
            .Produces<IEnumerable<WeatherForecast>>();
    }

    private static async Task<IResult> GetWeatherForecast(IMediator mediator)
    {
        var query = new GetWeatherForecastQuery();
        var forecast = await mediator.Send(query);
        return Results.Ok(forecast);
    }
}
```

## Naming Conventions

### Files and Folders
- ✅ Feature folders: **Plural** (e.g., `WeatherForecasts`, `Users`)
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
    cfg.RegisterServicesFromAssembly(typeof(GetWeatherForecastQuery).Assembly));
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
   app.MapGet("/weatherforecast", () => {
       var summaries = new[] { "Freezing", "Bracing" };
       // Business logic here
   });
   
   // ✅ GOOD
   app.MapGet("/weatherforecast", async (IMediator mediator) => {
       var query = new GetWeatherForecastQuery();
       return await mediator.Send(query);
   });
   ```

2. **Direct Database Access in Handlers**
   ```csharp
   // ❌ BAD - Direct DbContext access
   public class GetWeatherForecastQueryHandler {
       private readonly AppDbContext _context; // ❌
   }
   
   // ✅ GOOD - Use repository interface
   public class GetWeatherForecastQueryHandler {
       private readonly IWeatherForecastRepository _repository; // ✅
   }
   ```

3. **Skipping CQRS**
   ```csharp
   // ❌ BAD - Direct service call
   app.MapGet("/weatherforecast", (IWeatherService service) => {
       return service.GetForecasts();
   });
   
   // ✅ GOOD - Use MediatR query
   app.MapGet("/weatherforecast", async (IMediator mediator) => {
       return await mediator.Send(new GetWeatherForecastQuery());
   });
   ```

4. **Mixing Read and Write Operations**
   ```csharp
   // ❌ BAD - Query that modifies state
   public class GetWeatherForecastQueryHandler {
       public Task Handle(...) {
           _context.Logs.Add(...); // ❌ Modifying in query
       }
   }
   ```

## Checklist for New Features

When adding a new feature, ensure:

- [ ] Feature folder created in `Application/Features/{FeatureName}/`
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
