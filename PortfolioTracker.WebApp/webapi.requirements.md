# WebApi Project Requirements for PortfolioTracker

This document outlines the requirements and standards for implementing WebApi projects in the PortfolioTracker application.

## 1. Project Structure

Each WebApi project should follow this structure:
- **Models/** - Contains DTO models for API responses
- **Requests/** - Contains request handlers and endpoint extensions
- **Services/** - Domain-specific services (if needed)
- **Extensions/** - Extension methods for service registration (if needed)
- **Program.cs** - Main application entry point and configuration
- **appsettings.json** - Application configuration

## 2. Architecture

### 2.1 CQRS Pattern with MediatR

All WebApi projects must implement the Command Query Responsibility Segregation (CQRS) pattern using MediatR:
- **Commands** - For operations that change state (POST, PUT, DELETE)
- **Queries** - For operations that retrieve data (GET)

### 2.2 Minimal API Approach

Use .NET Minimal API approach with endpoint route builders:
- Group endpoints under a common route prefix (e.g., `/api/bankaccount`)
- Use extension methods for endpoint mapping
- Implement proper HTTP status code responses

### 2.3 Dependency Injection

- Register all services in the DI container
- Use transient or scoped lifetime as appropriate
- Inject dependencies via constructor injection

## 3. Implementation Requirements

### 3.1 Endpoint Mapping

Each endpoint should:
- Be defined in an extension method
- Have a clear naming convention
- Use proper HTTP methods (GET, POST, PUT, DELETE)
- Include appropriate route parameters
- Be documented with a name using `WithName()`

Example:
```csharp
public static void MapCreateEntity(this IEndpointRouteBuilder app)
{
    app.MapPost(@"/", CreateEntity)
        .WithName(nameof(CreateEntity));
}
```

### 3.2 Request/Response Pattern

- Each request should have a corresponding handler
- Use `IRequest<T>` for defining request contracts
- Implement `IRequestHandler<TRequest, TResponse>` for handlers
- Return appropriate result types (Ok, NotFound, BadRequest)

### 3.3 Data Access

- Use Entity Framework Core for data access
- Inject `IPortfolioContext` for database operations
- Implement proper exception handling
- Use async/await for all database operations

### 3.4 Models

- Use immutable models with `init` properties
- Implement proper validation
- Use `required` keyword for non-nullable properties
- Separate domain models from API response models

## 4. Cross-Cutting Concerns

### 4.1 Error Handling

- Implement proper exception handling in handlers
- Return appropriate HTTP status codes
- Log exceptions when they occur

### 4.2 Validation

- Validate input data before processing
- Return BadRequest for invalid inputs
- Use model validation attributes where appropriate

### 4.3 Documentation

- Enable Swagger for API documentation
- Document endpoints with appropriate descriptions
- Include example requests/responses

### 4.4 Security

- Implement appropriate authentication/authorization
- Protect sensitive endpoints
- Use HTTPS for all communications

## 5. Special Endpoints

### 5.1 Import Endpoints

For endpoints that handle imports (like `/import-transactions`):
- Use `[FromForm]` attribute for file uploads
- Disable antiforgery token validation when necessary with `.DisableAntiforgery()`
- Return the count or IDs of imported items
- Implement proper validation of imported data

### 5.2 Health Endpoints

Each WebApi should include a health endpoint:
```csharp
app.MapGet("/health", () => Results.Ok(@"ServiceName WebApi"));
```

## 6. Project References

Each WebApi project should reference:
- `PortfolioTracker.Data.Models` - For shared data models
- `PortfolioTracker.ServiceDefaults` - For common service configurations

## 7. Package Dependencies

Required NuGet packages:
- MediatR
- Microsoft.EntityFrameworkCore
- Npgsql.EntityFrameworkCore.PostgreSQL (for PostgreSQL access)
- Swashbuckle.AspNetCore (for Swagger)

## 8. Containerization

Each WebApi project should include:
- Dockerfile for containerization
- Docker configuration in launchSettings.json
