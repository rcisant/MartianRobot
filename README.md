# Introduction 
This template is aim to help developers to start writing applications using an N-Layers architecture desing suitable for most solutions.
The template include a lot of good examples about how to work with clean code, use of design patterns, unit testing, and best practices about how to work with ASP.Net Core API.

1. Install the latest [.NET Core SDK](https://dot.net).
2. Run `dotnet new -i sgs.aspnetcore.webapi.basic --nuget-source https://pkgs.dev.azure.com/sgs-dev-artifacts-feed/SGS-SWA-CoE-Feed/_packaging/SGS-SWA-CoE-Feed/nuget/v3/index.json` to install the project template.
3. Run `dotnet new sgs.webapi --help` to see how to select custom options of the project.
5. Run `dotnet new sgs.webapi -n MyProject` along with any other custom options to create a project from the template.

| Parameters        |          |         |                                                  |
|-------------------|----------|---------|--------------------------------------------------|
| -m\|--memoryCache | optional | false   | Adds in memory cache services to the target API. |

# Project structure

- **Api**: This project is the main entry point of the application and contains all the API behavior, the configuation, and the controllers of the application logic.
- **Application**: This project is where the application logic should be.
- **Domain**: This project holds the business model, which includes entities, business logic, and interfaces.
- **Host**: This project is the host of the Api project.
- **Infrastructure**: This project typically includes data access implementations and infrastructure-specific services (for example, `FileLogger` or `SmtpNotifier`)

## API
- **Swagger** Swagger is a format for describing the endpoints in your API and letting you try out your site using its user interface.
- **Versioning** Enable API versioning to version API endpoints.

## Continuous Integration (CI)

- **Azure DevOps** Adds Azure DevOps pipeline file 'azure-pipelines.yml' to continuous integration.

## Security

- **CORS** Browser security prevents a web page from making AJAX requests to another domain. This restriction is called the same-origin policy, and prevents a malicious site from reading sensitive data from another site. CORS is a W3C standard that allows a server to relax the same-origin policy. Using CORS, a server can explicitly allow some cross-origin requests while rejecting others.

## Analytics

- **HealthCheck** A health-check endpoint that returns the status of this API and its dependencies, giving an indication of its health.
- **Analytics** - Monitor internal information about how your application is running, as well as external user information.
  - **Application Insights** - Monitor internal information about how your application is running, as well as external user information using the Microsoft Azure cloud.
- **ApplicationInsightsKey** - Your Application Insights instrumentation key e.g. 11111111-2222-3333-4444-555555555555.

## Tests

- **UnitTest** - Unit test project.

## Docker support

- **Docker** Optimised Dockerfile to add the ability build a Docker image.

## Examples

- **Example Controller** - The example `InspectionController` contains the following actions:
  - GET - Returns single inspection by ID.
  - GET - Returns a list of inspections.
  - POST - Add a new inspection.
  - PUT - Update an existing inspection.
  - DELETE - Delete a single inspection, required rol `Administrator`.

## Logging

- **Serilog** - Has [Serilog](https://serilog.net/) built in for an excellent structured logging experience.

# Dependencies

- **SGS.Framework.Caching** *(optional) Provides classes to use memory caching facilities.* 
- **Automapper** *A convention-based object-object mapper.*
- **FluentValidation.AspNetCore** *A validation library for .NET that uses a fluent interface to construct strongly-typed validation rules.*
- **Hellang.Middleware.ProblemDetails** *Error handling middleware, using RFC7807.*
- **Polly** *(optional) Polly is a library that allows developers to express resilience and transient fault handling policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and Fallback in a fluent and thread-safe manner.*
- **Serilog** *Simple .NET logging with fully-structured events.*
  - Serilog.AspNetCore
  - Serilog.Enrichers.Environment
  - Serilog.Enrichers.Process
  - Serilog.Enrichers.Thread
  - Serilog.Exceptions
  - Serilog.Sinks.ApplicationInsights
- **Swashbuckle** *Seamlessly adds a Swagger to WebApi projects!*
  - Swashbuckle.AspNetCore
  - Swashbuckle.AspNetCore.Filters
  - Swashbuckle.AspNetCore.Swagger
  - Swashbuckle.AspNetCore.SwaggerUI
