- [Introduction](#introduction)
- [Usage Guide](#usage-guide)
    - [Prerequisites](#prerequisites)
    - [Infrastructure Dependencies](#infrastructure-dependencies)
    - [Configuration](#configuration)
    - [Database Configuration](#database-configuration)
    - [RabbitMQ Configuration](#rabbitmq-configuration)
    - [Email Testing Configuration](#email-testing-configuration)
    - [Running the Application](#running-the-application)
    - [Running Tests](#running-tests)
    - [Future To do List](#future-to-do-list)
---
<a id="top"></a>

# Introduction

# 🛒 ShoppingSystem Microservice
ShoppingSystem is an enterprise-level e-commerce microservice application built with **.NET 10 and C#**.

The project follows:

- Domain-Driven Design (DDD)
- Clean Architecture
- CQRS Pattern
- Microservice Architecture
- Event-Driven Architecture

The system uses an API Gateway with **YARP** as the entry point and communicates between services using **RabbitMQ message queues**.

> **Project Status: Under Development**
>
> ShoppingSystem is currently under active development. New features, improvements, and architectural enhancements are being added continuously.
>
> As the project is evolving, there may still be bugs, incomplete features, or areas that require further refinement. The codebase is continuously being improved with the goal of building a scalable, maintainable, and production-ready microservice architecture.
> 
> If you find any issues, have suggestions, or would like to contribute, please feel free to open an issue or fork the repository. Contributions and feedback are welcome and help improve the project as it continues to evolve.

---

## Architecture Overview
[⬆ Back to Top](#top)

ShoppingSystem follows **Microservice Architecture** combined with **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

The solution is organized into separate projects, where each project has a specific responsibility.


### Solution Structure

```text
ShoppingSystem.Microservice
│
├── ShoppingSystem.Microservice.Gateway
│   └── YARP Reverse Proxy
│
├── ShoppingSystem.Microservice.Domain
│   ├── Aggregates
│   ├── Entities
│   ├── Value Objects
│   └── Domain Events
│
├── ShoppingSystem.Microservice.Application
│   ├── CQRS Commands
│   ├── CQRS Queries
│   ├── MediatR Handlers
│   ├── DTOs
│   └── Validators
│
├── ShoppingSystem.Microservice.Infrastructure
│   ├── Entity Framework Core
│   ├── Repository Pattern
│   ├── Unit Of Work
│   ├── Database Factory
│   ├── RabbitMQ
│   └── Notification Abstractions
│
├── ShoppingSystem.Microservice.Infrastructure.Identity
│   ├── ASP.NET Core Identity
│   ├── JWT Authentication
│   ├── Refresh Tokens
│   └── OAuth Authentication
│
├── ShoppingSystem.Microservice.Notification.Email
│   ├── Email Service
│   ├── MailKit Integration
│   ├── Email Templates
│   └── Email Configuration
│
└── ShoppingSystem.Microservice.Notification.Sms
    ├── SMS Service
    ├── Twilio Integration
    ├── SMS Templates
    └── SMS Configuration
```

---
# Usage Guide
[⬆ Back to Top](#top)

This section explains how to configure, run, and use the ShoppingSystem Microservice application.

Before running the application, make sure all required dependencies are installed and infrastructure services are available.

---

## Prerequisites

[⬆ Back to Top](#top)

Before starting the project, install the following tools:

### Required Software

| Tool | Version |
|---|---|
| .NET SDK | .NET 10 |
| Docker Desktop | Latest |
| SQL Server | 2025 or Docker SQL Server Container |
| RabbitMQ | 3.x |
| Git | Latest |
| Visual Studio / Rider / VS Code | Latest |

### Recommended Tools

- SQL Server Management Studio (SSMS)
- Docker Desktop
- Postman 
- Navicat or another database management tool

---

# Infrastructure Dependencies
[⬆ Back to Top](#top)

ShoppingSystem requires several infrastructure services to run correctly.

The following dependencies are used:

| Service | Purpose |
|---|---|
| Database Provider | Application database storage |
| Identity Database | Authentication and user management (SQL Server only) |
| RabbitMQ | Event-driven communication between services |
| Mail Container | Email testing environment |

All infrastructure services can be started using Docker Compose.

---

## Docker Infrastructure
[⬆ Back to Top](#top)

The project provides Docker-based infrastructure for local development.

The infrastructure includes:

- SQL Server container
- RabbitMQ container
- Mail testing container

Example:

```bash
docker run -e "ACCEPT_EULA=Y" \
-e "MSSQL_SA_PASSWORD=YourStrong@Password123" \
-p 1433:1433 \
--name shopping-system-sqlserver \
-d mcr.microsoft.com/mssql/server:2025-latest
```

```bash
--hostname shopping-system-rabbitmq \
--name shopping-system-rabbitmq \
-p 5672:5672 \
-p 15672:15672 \
rabbitmq:3-management
```

```bash
docker run -d \
--name shopping-system-mailhog \
-p 1025:1025 \
-p 8025:8025 \
mailhog/mailhog
```
http://localhost:8025/

This is the UI for MailHog when you open it in the browser:
![img.png](img.png)

# Configuration

[⬆ Back to Top](#top)

ShoppingSystem uses ASP.NET Core configuration providers to manage application settings.

Configuration can be provided through:

- `appsettings.json`
- `appsettings.Development.json`
- User Secrets
- Environment Variables

# Database Configuration

[⬆ Back to Top](#top)

ShoppingSystem supports configurable database providers for the main application, allowing the system to work with different database engines based on deployment requirements.

Supported Providers
SQL Server
PostgreSQL
MySQL
In-Memory Database (for testing and development scenarios)
Selecting a Database Provider

The active database provider is configured using User Secrets.

For PostgreSQL:
```
dotnet user-secrets set "Database:Provider" "PostgresDb"
```
For SQL Server:
```
dotnet user-secrets set "Database:Provider" "SqlServer"
```
For MySQL:
```
dotnet user-secrets set "Database:Provider" "MySql"
```
You should also configure the corresponding connection string in your User Secrets:

dotnet user-secrets set "ConnectionStrings:ShoppingSystem_Microservice" "<your_connection_string>"
Important: Provider-Specific Migrations

Entity Framework Core migrations are database-provider specific. A migration generated for one provider (for example, SQL Server) cannot be reused for another provider (such as PostgreSQL or MySQL).

Each supported provider should maintain its own migration history in a separate folder. A recommended structure is:

Infrastructure
```
└── Persistence
└── Migrations
├── SqlServer
├── Postgres
├── MySql
└── InMemory
```

When switching to a different database provider:

Set the desired provider using User Secrets.
Generate migrations for that provider only.
Apply the migrations to the corresponding database.

Keeping migrations separated ensures that provider-specific features (such as SQL functions, filtered indexes, generated columns, and data types) are generated correctly and remain compatible with each database engine.


Important: The IdentityConnectionStringName must always point to a SQL Server database because the Identity service uses ASP.NET Core Identity with SQL Server as its storage provider.

Current configuration:

```json
{
  "Database": {
    "Provider": "SqlServer",
    "ConnectionStringName": "ShoppingSystem_Microservice",
    "IdentityConnectionStringName": "ShoppingSystem_Microservice_Identity"
  }
}

{
  "ConnectionStrings": {
    "ShoppingSystem_Microservice": "Your connection string",
     "ShoppingSystem_Microservice_Identity": "Your connection string"
}
```
# RabbitMQ Configuration
[⬆ Back to Top](#top)

ShoppingSystem uses RabbitMQ as the message broker for event-driven communication between microservices.

RabbitMQ is responsible for:

- Publishing integration events
- Consuming messages from queues
- Asynchronous communication between services
- Decoupling microservices


The RabbitMQ configuration is defined in `appsettings.json`:

```json
{
  "RabbitMq": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest"
  }
}
```
# Email Testing Configuration
[⬆ Back to Top](#top)

ShoppingSystem uses **MailKit** for sending email notifications.

For local development and testing, the project uses a mail testing container that provides an SMTP server. Emails are captured locally and can be viewed through a browser interface without sending real emails.

The email configuration is defined in `appsettings.json`:

```json
{
  "EmailSettings": {
    "Host": "localhost",
    "Port": 1025,
    "From": "test@local.com",
    "DisplayName": "ShoppingSystem.Support",
    "UseSsl": false
  }
}
```
After starting the required Docker dependencies, you can access the web interface (GUI) at:

http://localhost:8025

# Running the Application
[⬆ Back to Top](#top)

Before running the application, make sure all required infrastructure dependencies are available:

- Database server
- RabbitMQ
- Email testing service

The application requires the database to be configured and migrations to be applied before starting the services.

## Running with YARP API Gateway

If you want to use the YARP API Gateway, you must run both the gateway and the required application services together.

The API Gateway acts as the single entry point for client requests and routes incoming traffic to the appropriate microservices.

After starting:

- `ShoppingSystem.Microservice.Gateway`
- Required backend services

You should send your API requests through the gateway instead of calling the services directly.

```
https://localhost:7233/api/1/Address/GetAll?Criteria.PageSize=10&Criteria.PageNumber=1
```
Or:
```
https://localhost:7233/health
```

---
# Running Tests
[⬆ Back to Top](#top)

ShoppingSystem includes automated tests to verify the correctness and reliability of the application.

The test projects cover different layers of the system, including:

- Domain logic
- Application services
- Business rules
- Integration scenarios


## Running All Tests

---
# Future To Do List
[⬆ Back to Top](#top)

ShoppingSystem is continuously evolving. The following features and improvements are planned for future development.

| Category | Planned Improvement |
|---|---|
| Infrastructure | Fully containerize the complete application using Docker |
| Infrastructure | Containerize all microservices |
| Infrastructure | Containerize infrastructure dependencies |
| Infrastructure | Create a complete Docker Compose environment for local development |
| Infrastructure | Improve deployment consistency across environments |
| Deployment | Add Kubernetes deployment support |
| Deployment | Add container orchestration |
| Deployment | Improve environment-based configuration management |
| Performance | Add Redis caching support |
| Performance | Cache frequently accessed data |
| Performance | Improve API response times |
| Performance | Reduce unnecessary database queries |
| Performance | Implement distributed caching between services |
| Performance | Add response caching where applicable |
| Security | Implement API rate limiting |
| Security | Protect APIs against excessive requests |
| Security | Prevent abuse and unnecessary resource consumption |
| Security | Add configurable rate limit policies |
| Security | Improve secret management |
| Security | Move sensitive configuration to secure secret providers |
| Security | Improve production authentication security |

---
