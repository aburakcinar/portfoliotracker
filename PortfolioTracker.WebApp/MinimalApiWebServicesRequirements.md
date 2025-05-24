# Minimal API Web Services Requirements

This document describes the requirements and conventions for creating new minimal API-based WebApi projects in the PortfolioTracker solution. Follow these guidelines to ensure consistency and maintainability.

---

## 1. Project Structure

- Project/Solution root is where PortfolioTracker.sln file is located.
- Each WebApi project should have its own directory under `PortfolioTracker.WebApp` (e.g., `PortfolioTracker.Assets.WebApi`).
- The following subdirectories are recommended:
  - `Requests`: For endpoint mapping  request/handler classes.
     - 'Requests' folder should have all the requests/commands and their handlers and also Router extension on top of the file. 
  - `Extensions`: For endpoint mapping extensions if needed.
  - `Models`: For API-specific models or DTOs.
  - `Properties`: For launch settings and project metadata.
- Include a `Dockerfile` for containerization.
- Include `appsettings.json` and `appsettings.Development.json` for configuration.
- Don't remove/delete any existing folders. 
- Created domain project should be available for aspire host.

---

## 2. Dependencies and Configuration

- Use `Microsoft.NET.Sdk.Web` as the project SDK.
- Register services in Program.cs:
  - `AddServiceDefaults()` for shared configuration.
  - `AddEndpointsApiExplorer()` and `AddSwaggerGen()` for OpenAPI/Swagger support.
  - `AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Placeholder>())` for CQRS/mediator pattern.
  - Register a `DbContext` (e.g., `PortfolioContext`) using `UseNpgsql` for PostgreSQL.
  - Register an interface abstraction for the DbContext (e.g., `IPortfolioContext`).
- Use central package version management for package versions (via `Directory.Packages.props`).

---

## 3. Program.cs Patterns

- Use minimal API approach; do not use controllers.
- Group endpoints using `app.MapGroup("/api/{entity}")`.
- Map endpoint extensions (e.g., `mapGroup.MapGetAsset()`) for each logical operation.
- Always provide a `/health` endpoint returning a simple string for health checks.
- Use Swagger UI in development mode.
- End with `app.Run();`.

---

## 4. Endpoint Mapping

- Define endpoints as extension methods on `IEndpointRouteBuilder` in the `Requests` folder on top of the related request file.
- Search the PortfolioTracker.WebApi project for the related xxxRequestHandler class and move it to the related project.
- Use MediatR request/response pattern for business logic.
- File name should be same name with the xxxRequestHandler class name.
- Example extension method:
  ```csharp
  public static void MapGetAsset(this IEndpointRouteBuilder app)
  {
      app.MapGet("/{id}", GetAsset)
         .WithName(nameof(GetAsset));
  }
  ```
- Example endpoint handler:
  ```csharp
  private static async Task<IResult> GetAsset(IMediator mediator, int id)
  {
      var result = await mediator.Send(new GetAssetRequest { Id = id });
      return TypedResults.Ok(result);
  }
  ```

---

## 5. Naming Conventions

- Project names must follow the pattern: `PortfolioTracker.{Domain}.WebApi`
- {Domain} must be in plural form.
- Namespace must match the project name.
- Endpoint group route: `/api/{domain}` (e.g., `/api/asset`).
- Endpoints must be named in singular form.

---

## 6. CQRS and MediatR

- Requests and handlers are placed in the `Requests` folder.
- Each endpoint should have a corresponding request and handler class.
- Handlers should use dependency injection for database/context access.

---

## 7. Docker Support

- Provide a `Dockerfile` in each WebApi project.
- Expose ports 80 and 443.
- Use multi-stage build for publishing and runtime.

---

## 8. App Settings

- Provide `appsettings.json` and `appsettings.Development.json` with at least the connection string for the database.

---

## 9. Service Defaults

- Use `AddServiceDefaults()` and `MapDefaultEndpoints()` for consistent cross-cutting concerns (e.g., logging, health checks).

---

## 10. Placeholder Class

- Include a `Placeholder` class in Program.cs to assist with MediatR assembly scanning.

## 11. Validation

- Run build for target project and validate there is no build error.


