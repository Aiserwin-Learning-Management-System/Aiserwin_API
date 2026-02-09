# Winfocus LMS – Developer Documentation

## 1. Tech Stack

### Backend

* **.NET 10 (ASP.NET Core Web API)**
* **C#**
* **Entity Framework Core** (Code First)
* **SQL Server**
* **JWT Authentication** (Access Tokens)
* **Role-Based Authorization**
* **Serilog** for structured logging

### Architecture

* **Clean Architecture**
* **Domain-Driven Design (DDD)**
* **SOLID Principles**

### Tooling

* .NET SDK 10
* SQL Server / SQL Server Express
* Visual Studio
* Git & GitHub

---

## 2. Solution Structure

```
Winfocus.LMS.sln
│
├── src
│   ├── Winfocus.LMS.Api            # Presentation layer (Controllers, Middleware)
│   ├── Winfocus.LMS.Application    # Application layer (Use cases, DTOs, Interfaces)
│   ├── Winfocus.LMS.Domain         # Domain layer (Entities, Enums, Value Objects)
│   └── Winfocus.LMS.Infrastructure # Infrastructure layer (EF Core, Identity, JWT, Logging)
│
├── tests
│   ├── Winfocus.LMS.Api.Tests
│   ├── Winfocus.LMS.Application.Tests
│   └── Winfocus.LMS.Infrastructure.Tests
│
└── README.md
```

### Layer Responsibilities

#### Domain

* Pure business logic
* Entities, Enums, Base Entities
* No dependency on other layers

#### Application

* Use cases and business workflows
* DTOs, Interfaces, Validators
* Depends only on **Domain**

#### Infrastructure

* Database access (EF Core)
* Identity & Authentication
* External services (JWT, Logging)
* Implements Application interfaces

#### API

* Controllers
* Middleware (Exception Handling, Logging)
* Dependency Injection
* No business logic

---

## 3. How to Run Locally

### Prerequisites

* .NET SDK 10 installed
* SQL Server running locally

### Steps

1. Clone the repository

```bash
git clone <repo-url>
cd Winfocus.LMS
```

2. Configure database connection

Update `appsettings.Development.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=WinfocusLmsDb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=\"SQL Server Management Studio\";Command Timeout=0"
}
```

3. Apply database migrations

```bash
dotnet ef database update --startup-project ../Winfocus.LMS.Api --project .
```

4. Run the API

```bash
dotnet run --project src/Winfocus.LMS.Api
```

5. Swagger UI

```
https://localhost:5001/swagger
```

---

## 4. Database & Migration Strategy

* **EF Core Code-First** approach
* All migrations live in `Infrastructure` layer
* `AppDbContext` exists only in Infrastructure
* API and Application never reference EF Core directly

### Add Migration

```bash
dotnet ef migrations add <MigrationName> \
 --project src/Winfocus.LMS.Infrastructure \
 --startup-project src/Winfocus.LMS.Api
```

### Update Database

```bash
dotnet ef database update \
 --project src/Winfocus.LMS.Infrastructure \
 --startup-project src/Winfocus.LMS.Api
```

---

## 5. Authentication & Authorization

* JWT-based authentication using ASP.NET Core JWT Bearer middleware
* Access tokens issued on successful login
* Role-based authorization via `[Authorize(Roles = "...")]`

### Token Flow

1. User logs in
2. JWT issued with UserId & Roles
3. Token validated via middleware
4. Claims used for authorization

**Note:** Concrete authentication and token services are wired in the API composition root (`Program.cs`).

---

## 6. Logging & Exception Handling

### Logging

* **Serilog** configured in `Program.cs`
* Console and rolling file sinks enabled
* File retention and size limits configured

### Exception Handling

* Centralized global exception middleware registered first in the pipeline
* All unhandled exceptions are logged and returned as safe error responses

---

## 7. Coding Standards (Mandatory)

### Enforced via `.editorconfig`

* 4-space indentation, braces required
* `var` usage disallowed
* Accessibility modifiers required
* Private fields must use `_camelCase`
* StyleCop analyzers enabled

### XML Documentation

* XML documentation is mandatory for classes, methods, interfaces, parameters, and return values
* Missing documentation produces **build errors** when analyzers are enabled

### Error Handling

* Try-catch required in critical operations
* Exceptions must be logged with context

### Architecture Notes

* API acts as the **composition root**
* Infrastructure implementations are wired in `Program.cs`
* Application layer depends only on abstractions

---

## 8. Testing

* xUnit is the preferred testing framework
* Test project structure may include:

  * API tests
  * Application tests
  * Infrastructure tests

> Note: Test coverage and structure depend on current implementation status.

---

## 9. Contribution Guidelines

* Follow Clean Architecture strictly
* Add logs for every important operation
* Add unit tests for new features
* Ensure migrations are reviewed before merge

---

## 10. Notes for New Developers

* Read **Domain first**, then Application
* Never access `AppDbContext` outside Infrastructure
* Use interfaces for all external dependencies
* When in doubt, prefer clarity over shortcuts
