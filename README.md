# Foundatio.CommandQuery

[![NuGet](https://img.shields.io/nuget/v/Foundatio.CommandQuery.svg)](https://www.nuget.org/packages/Foundatio.CommandQuery/)
[![Build Status](https://github.com/FoundatioFx/Foundatio.CommandQuery/workflows/Build/badge.svg)](https://github.com/FoundatioFx/Foundatio.CommandQuery/actions)

A powerful Command Query Responsibility Segregation (CQRS) framework built on the mediator pattern. This library provides core abstractions and implementations for building scalable, maintainable applications using CQRS principles.

## Features

- **Clean CQRS Implementation** - Separate read and write operations with clear command/query patterns
- **Mediator Pattern** - Built on [Foundatio.Mediator](https://github.com/FoundatioFx/Foundatio.Mediator) for decoupled messaging
- **Multiple Data Stores** - Support for Entity Framework Core and MongoDB out of the box
- **ASP.NET Core Integration** - Minimal API endpoints for exposing commands and queries via HTTP
- **Caching Support** - Built-in caching abstractions for query optimization
- **Dynamic Queries** - Powerful filtering, sorting, and pagination with LINQ support
- **Multi-Targeting** - Supports .NET 8, .NET 9, and .NET 10

## Packages

| Package                                    | Description                                                                                                   | NuGet                                                                                                                                                         |
| ------------------------------------------ | ------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Foundatio.CommandQuery**                 | Core abstractions and base classes for implementing CQRS patterns using the mediator pattern                  | [![NuGet](https://img.shields.io/nuget/v/Foundatio.CommandQuery.svg)](https://www.nuget.org/packages/Foundatio.CommandQuery/)                                 |
| **Foundatio.CommandQuery.Dispatcher**      | Dispatcher implementation providing centralized command and query handling with caching support               | [![NuGet](https://img.shields.io/nuget/v/Foundatio.CommandQuery.Dispatcher.svg)](https://www.nuget.org/packages/Foundatio.CommandQuery.Dispatcher/)           |
| **Foundatio.CommandQuery.Endpoints**       | ASP.NET Core Minimal API endpoints for HTTP-based command and query execution                                 | [![NuGet](https://img.shields.io/nuget/v/Foundatio.CommandQuery.Endpoints.svg)](https://www.nuget.org/packages/Foundatio.CommandQuery.Endpoints/)             |
| **Foundatio.CommandQuery.EntityFramework** | Entity Framework Core integration providing database query and persistence handlers with dynamic LINQ support | [![NuGet](https://img.shields.io/nuget/v/Foundatio.CommandQuery.EntityFramework.svg)](https://www.nuget.org/packages/Foundatio.CommandQuery.EntityFramework/) |
| **Foundatio.CommandQuery.MongoDB**         | MongoDB integration providing NoSQL database query and persistence handlers with dynamic LINQ support         | [![NuGet](https://img.shields.io/nuget/v/Foundatio.CommandQuery.MongoDB.svg)](https://www.nuget.org/packages/Foundatio.CommandQuery.MongoDB/)                 |

## Installation

### Using .NET CLI

```bash
# Core package
dotnet add package Foundatio.CommandQuery

# Entity Framework integration
dotnet add package Foundatio.CommandQuery.EntityFramework

# MongoDB integration
dotnet add package Foundatio.CommandQuery.MongoDB

# ASP.NET Core endpoints
dotnet add package Foundatio.CommandQuery.Endpoints

# Dispatcher for Blazor 
dotnet add package Foundatio.CommandQuery.Dispatcher
```

### Using Package Manager Console

```powershell
Install-Package Foundatio.CommandQuery
Install-Package Foundatio.CommandQuery.EntityFramework
Install-Package Foundatio.CommandQuery.MongoDB
Install-Package Foundatio.CommandQuery.Endpoints
Install-Package Foundatio.CommandQuery.Dispatcher
```

## Quick Start

### 1. Define Your Models

```csharp
// Entity model
public class Task : IHaveIdentifier<int>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public string CreatedBy { get; set; }
}

// Read model
public class TaskReadModel : EntityReadModel<int>
{
    public string Title { get; set; }
    public string Description { get; set; }
}

// Create model
public class TaskCreateModel : EntityCreateModel<int>
{
    public string Title { get; set; }
    public string Description { get; set; }
}

// Update model
public class TaskUpdateModel : EntityUpdateModel
{
    public string Title { get; set; }
    public string Description { get; set; }
}
```

### 2. Create Command and Query Handlers

```csharp
using Foundatio.CommandQuery.EntityFramework;

public class TaskCommandHandler 
    : EntityCommandHandler<AppDbContext, Task, int, TaskReadModel, TaskCreateModel, TaskUpdateModel>
{
    public TaskCommandHandler(AppDbContext context, IMapper mapper) 
        : base(context, mapper)
    {
    }
}

public class TaskQueryHandler 
    : EntityQueryHandler<AppDbContext, Task, int, TaskReadModel>
{
    public TaskQueryHandler(AppDbContext context, IMapper mapper) 
        : base(context, mapper)
    {
    }
}
```

### 3. Register Services

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add mediator with assemblies containing handlers
builder.Services.AddMediator(options => options
    .AddAssembly(typeof(Program).Assembly)
);

// Add your DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

// Add endpoint routes
builder.Services.AddEndpointRoutes();
```

### 4. Configure Minimal API Endpoints

```csharp
public class TaskEndpoint 
    : EntityCommandEndpointBase<int, TaskListModel, TaskReadModel, TaskCreateModel, TaskUpdateModel>
{
    public TaskEndpoint(ILoggerFactory loggerFactory) 
        : base(loggerFactory, "tasks", "api/tasks")
    {
    }
}

var app = builder.Build();

// Map endpoint routes
app.MapEndpointRoutes();

app.Run();
```

This automatically creates the following endpoints:

- `GET /api/tasks/{id}` - Get a task by ID
- `GET /api/tasks` - Query tasks with pagination and sorting
- `POST /api/tasks/query` - Query tasks with advanced filtering
- `GET /api/tasks/{id}/update` - Get task update model
- `POST /api/tasks` - Create a new task
- `PUT /api/tasks/{id}` - Update a task
- `DELETE /api/tasks/{id}` - Delete a task

## Advanced Usage

### Using Commands Directly

```csharp
public class TaskService
{
    private readonly IMediator _mediator;

    public TaskService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<TaskReadModel>> CreateTaskAsync(TaskCreateModel model, ClaimsPrincipal user)
    {
        var command = new CreateEntity<TaskCreateModel, TaskReadModel>(user, model);
        return await _mediator.InvokeAsync<Result<TaskReadModel>>(command);
    }

    public async Task<Result<TaskReadModel>> GetTaskAsync(int id, ClaimsPrincipal user)
    {
        var query = new GetEntity<int, TaskReadModel>(user, id);
        return await _mediator.InvokeAsync<Result<TaskReadModel>>(query);
    }

    public async Task<Result<QueryResult<TaskReadModel>>> QueryTasksAsync(
        QueryDefinition query, 
        ClaimsPrincipal user)
    {
        var command = new QueryEntities<TaskReadModel>(user, query);
        return await _mediator.InvokeAsync<Result<QueryResult<TaskReadModel>>>(command);
    }
}
```

### Advanced Querying

```csharp
// Query with filtering, sorting, and pagination
var queryDefinition = new QueryDefinition
{
    Filter = new QueryFilter
    {
        Logic = FilterLogic.And,
        Filters = new List<QueryFilter>
        {
            new() { Field = "Status", Operator = FilterOperator.Equal, Value = "Active" },
            new() { Field = "Priority", Operator = FilterOperator.GreaterThan, Value = 3 }
        }
    },
    Sorts = new List<QuerySort>
    {
        new() { Name = "Priority", Descending = true },
        new() { Name = "Created", Descending = false }
    },
    Page = 1,
    PageSize = 20
};

var query = new QueryEntities<TaskReadModel>(user, queryDefinition);
var result = await _mediator.InvokeAsync<Result<QueryResult<TaskReadModel>>>(query);
```

### MongoDB Usage

```csharp
using Foundatio.CommandQuery.MongoDB;

public class TaskCommandHandler 
    : EntityCommandHandler<Task, int, TaskReadModel, TaskCreateModel, TaskUpdateModel>
{
    public TaskCommandHandler(IMongoDatabase database, IMapper mapper) 
        : base(database, mapper)
    {
    }
}

public class TaskQueryHandler 
    : EntityQueryHandler<Task, int, TaskReadModel>
{
    public TaskQueryHandler(IMongoDatabase database, IMapper mapper) 
        : base(database, mapper)
    {
    }
}
```

### Custom Command Handlers

```csharp
public record ProcessTaskCommand : PrincipalCommand<CompleteModel>
{
    public ProcessTaskCommand(ClaimsPrincipal? principal, string action) 
        : base(principal)
    {
        Action = action;
    }

    public string Action { get; }
}

public class ProcessTaskHandler : IRequestHandler<ProcessTaskCommand, CompleteModel>
{
    public async ValueTask<CompleteModel> HandleAsync(
        ProcessTaskCommand request, 
        CancellationToken cancellationToken = default)
    {
        // Custom processing logic
        return CompleteModel.Success($"Processed: {request.Action}");
    }
}
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.
