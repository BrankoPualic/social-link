# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

### Backend (.NET 9)

```bash
# Build
dotnet build "SocialLink.sln"

# Run API
dotnet run --project SocialLink.Web

# Watch mode
dotnet watch run --project SocialLink.Web

# Tests
dotnet test "SocialLink.sln"

# EF Core migrations (run from the module's project directory)
dotnet ef migrations add <MigrationName> --context <ContextName>
dotnet ef database update --context <ContextName>
```

Context names: `UserDatabaseContext`, `PostDatabaseContext` — each module has its own context.

### Frontend (Angular 20)

```bash
cd SocialLink.Web.Client/Workspace

npm install
npm start       # dev server at https://localhost:4200 (HTTPS required)
npm run build   # production build
npm test        # Karma + Jasmine unit tests
```

SSL certs required for local dev at `SocialLink.Web.Client/Workspace/ssl/server.key` and `server.crt`.

### API Manual Testing

HTTP test files live at `SocialLink.Web/Tests/*.web.http` (VS Code REST Client format) — one file per module.

## Architecture

**SocialLink** is a modular monolith social media platform. The backend is ASP.NET Core 9 and the frontend is an Angular 20 SPA served via the `SocialLink.Web.Client` project (Angular dev proxy → .NET API).

### Module structure

Every feature lives under `src/Modules/[Name]/`:

```
SocialLink.[Module]/
├── Application/UseCases/Commands/   # MediatR commands + handlers
├── Application/UseCases/Queries/    # MediatR queries + handlers
├── Application/UseCases/Validators/ # FluentValidation rules
├── Application/Services/            # Business logic
├── Controllers/                     # ASP.NET controllers
├── Data/                            # EF Core / MongoDB contexts + repos
├── Domain/                          # Entities
├── Integrations/                    # Handles contracts requested by other modules
└── [Module]Module.cs                # DI registration entry point
```

Current modules: **Users**, **Posts**, **Notifications**, **Blobs**, **Messaging**.

Each module also has a sibling `SocialLink.[Module].Contracts/` project containing public interfaces that other modules are allowed to call via MediatR.

### Cross-module rules

- Module internals must be `internal`. Public surface is exposed only through the `.Contracts` project.
- Cross-module calls go through MediatR: the calling module sends a query defined in the target's `.Contracts`; the handler lives in the target module's `Integrations/` folder.
- No direct project references between feature modules — only through Contracts.

### Infrastructure

| Concern | Technology |
|---|---|
| Relational data | SQL Server + EF Core 9 |
| Unstructured data | MongoDB (Notifications, Blobs, Messages) |
| Auth | JWT Bearer, stored in HTTP-only cookies |
| Real-time | SignalR (presence + messaging) |
| CQRS mediator | MediatR 13 |
| Validation pipeline | FluentValidation 12 |
| Logging | Serilog |
| File storage | Azure Blob Storage |
| Secrets | Azure Key Vault + environment variables |

### Configuration

- `SocialLink.Web/appsettings.json`: Serilog, connection strings
  - SQL Server default: `Data Source=localhost; Initial Catalog=SocialLink`
  - MongoDB databases: `SocialLinkNotifications`, `SocialLinkBlobs`, `SocialLinkMessages`
- CORS is configured for `https://localhost:4200` only in development.
- Secrets (JWT key, connection strings) come from Azure Key Vault or environment variables — not from appsettings.

### Shared projects

- `SocialLink.SharedKernel` — domain primitives, constants, helpers used across all modules
- `SocialLink.Common.Application` — base repository interfaces, base DTOs, application-layer interfaces
- `SocialLink.Common.Data` — EF Core and MongoDB base classes (generic repo implementations)

### Adding a backend feature (typical flow)

1. Add/update entity in `Domain/`
2. Create Command or Query + handler in `Application/UseCases/`
3. Add FluentValidation validator in `Application/UseCases/Validators/`
4. Expose via controller in `Controllers/`
5. If other modules need to call it: add interface to `.Contracts`, implement handler in `Integrations/`
6. If schema changed: add EF migration
