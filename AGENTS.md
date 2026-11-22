# AGENTS.md

Guidance for AI coding agents working in this repository.

## Project Overview

**WebApplicationRestaurant** is an ASP.NET Core 11 (`net11.0`, C# 15) MVC web application for restaurant operations management (UI text, `[Display]` attributes, and validation messages are in Ukrainian).

- **Framework**: ASP.NET Core MVC 11, Entity Framework Core 11.0.0 (PostgreSQL via `Npgsql.EntityFrameworkCore.PostgreSQL` 11.0.0 + `EFCore.NamingConventions` 11.0.0), ASP.NET Core Identity
- **Observability**: OpenTelemetry (`OpenTelemetry.Extensions.Hosting` 1.21.0, `OpenTelemetry.Exporter.OpenTelemetryProtocol` 1.21.0, `OpenTelemetry.Instrumentation.AspNetCore`/`Http`/`Runtime` 1.21.0, `Npgsql.OpenTelemetry` 11.0.0) across logs, metrics, and traces
- **Language settings**: `<Nullable>enable</Nullable>`, `<ImplicitUsings>enable</ImplicitUsings>`, `<WarningsAsErrors>nullable</WarningsAsErrors>`, file-scoped namespaces, primary constructors, collection expressions (`[...]`)
- **Solution / Project**: `WebApplicationRestaurant.sln` / `WebApplicationRestaurant.csproj`
- **Client libraries**: Bootstrap 4.6.0 (via `libman.json`), jQuery, jQuery Validation in `wwwroot/lib/` (served via `MapStaticAssets()` / `.WithStaticAssets()`)

## Build, Run & Test Commands

```powershell
# Restore & build
dotnet build

# Clean build without restoring
dotnet build --no-incremental --no-restore

# Run the web app (HTTPS: https://localhost:5001, HTTP: http://localhost:5000)
dotnet run

# Optional: start standalone .NET Aspire Dashboard (UI: http://localhost:18888, OTLP gRPC: 4317)
docker compose -f docker-compose.telemetry.yml up -d
# Run with OTLP export enabled
OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317 dotnet run
```

- **Tests**: There is currently no test project in the repository (`WebApplicationRestaurant.csproj` already excludes `WebApplicationRestaurant.Tests/**` and `AGENTS.md`).
- **EF Core migrations**: PostgreSQL migrations live in `Migrations/` (`InitialCreate`). `Microsoft.EntityFrameworkCore.Design` is referenced explicitly alongside `Microsoft.EntityFrameworkCore.Tools` (required in EF Core 11).

## IDE & MCP Tooling

- **Prioritize JetBrains Rider MCP** (`jetbrains-rider` configured in `.mcp.json` / `.air/mcp.json`, or `jetbrains_air_ide` via `execute_tool` / `post_edit_quality_check` when running inside Air/Rider):
  - **Code navigation & symbol lookups**: Prefer `search_symbol`, `find_usages`, `find_implementations`, and `get_symbol_info` over text/grep searches.
  - **Refactoring & formatting**: Prefer Rider refactoring tools (`rename_refactoring`, `move_type_to_namespace`, `extract_method`, `reformat_file`) over manual multi-file edits.
  - **Diagnostics & builds**: Prefer `get_file_problems`, `lint_files`, `get_project_problems`, `post_edit_quality_check`, and `build_solution_start` / `build_solution_state` for IDE inspections and builds, falling back to CLI `dotnet build` only if Rider MCP is unavailable or disconnected.
- **Microsoft Learn MCP (`.mcp.json` & `.agents/mcp_config.json`)**: Configured with `microsoft-learn` (`https://learn.microsoft.com/api/mcp`) for up-to-date ASP.NET Core 11, EF Core 11, Identity, and C# 15 documentation and code samples (`microsoft_docs_search`, `microsoft_docs_fetch`, `microsoft_code_sample_search`). `.agents/mcp_config.json` is loaded by Google Antigravity, while `.mcp.json` is loaded by Rider/Air, Claude Code, and Cursor.
- **Repository Skills & Custom Agents**: Curated skills live in `.agents/skills/` (Microsoft Learn docs/reference/CLI, EF Core query optimization, data-driven ASP.NET Core MVC scaffolding, C# refactoring, .NET performance analysis, `System.Text.Json` .NET 11, and .NET testing workflows). Custom agents (`optimizing-dotnet-performance`, `test-engineer`, `msbuild-code-review`) live in `.agents/agents/*.md` (Antigravity-native frontmatter) and `.github/agents/*.agent.md` (GitHub Copilot / IDE format). See `.agents/SOURCES.md` for upstream sources and pinned versions.

## Architecture & Directory Structure

- `Program.cs`: Top-level statements using `WebApplication.CreateBuilder(args)`. Configures observability (`builder.AddRestaurantObservability()`), `ApplicationContext` (`UseNpgsql` + `UseSnakeCaseNamingConvention`), Identity (relaxed password rules: min length 5, no digit/case/symbol requirements), MVC (`SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true` to preserve attribute-driven form validation), `DistributedMemoryCache` + `Session`, `ru-RU` culture with `.` as decimal separator, `TelemetryContextMiddleware` (after `UseAuthorization()`), `MapStaticAssets()`, and MVC routes (`areas` + `default`). Also runs `SampleData.Initialize` and `UserRoleInitializer.InitializeAsync` inside an async DI scope on startup.
- `Observability/`: Unified OpenTelemetry setup and application instrumentation:
  - `OpenTelemetryExtensions.cs`: Configures `SimpleConsole` (Development) / `JsonConsole` (Production), OpenTelemetry log bridge (`IncludeFormattedMessage`, `IncludeScopes`, `ParseStateValues`), tracing (`AspNetCore`, `HttpClient`, `Npgsql`, custom `ActivitySource`), metrics (`AspNetCore`, `HttpClient`, `Runtime`, `Npgsql`, custom `Meter`), and opt-in `UseOtlpExporter(...)`.
  - `RestaurantTelemetry.cs`: Shared `ActivitySource` and `Meter` (`WebApplicationRestaurant`) defining business metrics (`restaurant.auth.logins`, `restaurant.orders.created`, `restaurant.orders.items_count`, `restaurant.orders.closed`, `restaurant.dishes.status_changes`, `restaurant.tables.reservations`, `restaurant.ingredient_orders.created`, `restaurant.ingredient_orders.closed`, `restaurant.admin.user_events`).
  - `LogEvents.cs`: Source-generated `[LoggerMessage]` structured log methods (`EventId` 1001–5004) used across `Program.cs` and instrumented controllers.
  - `TelemetryContextMiddleware.cs`: Enriches `Activity.Current` and `ILogger` scopes with authenticated user metadata (`UserId`, `UserName`, `UserRoles`) and MVC route values (`Area`, `Controller`, `Action`) without logging secrets/PII.
- `docker-compose.telemetry.yml`: Optional standalone `.NET Aspire Dashboard` container for inspecting OTLP logs, traces, and metrics locally.
- `SessionExtensions.cs`: `ISession.Set<T>` / `Get<T?>` JSON serialization helpers (`System.Text.Json`).
- `Data/ApplicationContext.cs`: `IdentityDbContext<User>` (primary constructor) configured for PostgreSQL with full `snake_case` naming (domain tables, `recipes`/`menu_plan_items`/`schedule_places` join tables, `shopping_cart_items`, `order_ingredients_items`, and explicitly mapped `asp_net_*` Identity tables and indexes). String `Name`/short text columns use `varchar(20)` (`HasMaxLength(20)` without `IsFixedLength`); domain `DateTime` columns use `timestamp without time zone` with `LOCALTIMESTAMP` defaults for `orders.create_time` and `order_ingredients.create_date`.
- `Controllers/`:
  - `HomeController.cs`: Redirects unauthenticated users to `Account/Login`; renders role-specific partial links (`Views/Home/*Links.cshtml`). For `waiter`, loads today's `Schedule.Places` into `ViewBag.Places` (falling back to `[]` if none scheduled). `Error()` logs unhandled exceptions via `IExceptionHandlerPathFeature` and sets error status on `Activity.Current`.
  - `AccountController.cs`: Login and logout actions (instrumented with `LogEvents` and `restaurant.auth.logins`).
- `Areas/`: Role-separated MVC areas:
  - `Areas/Administration/`: System/catalog management (`Categories`, `Dishes` + `EditRecipe`, `DishStatuses`, `DishUnits`, `Ingredients`, `IngredientUnits`, `OrderStatuses`, `OrderIngredientsStatuses`, `ChangingOrderIngredientsStatuses`, `Roles`, `Users`).
  - `Areas/RestaurantAdministration/`: `MenuPlans` (includes `EditMenuDishes`) and `Schedules` (includes `EditSchedulePlaces`).
  - `Areas/Cooker/`: `ChangingDishStatuses` (dish prep status workflow) and `OrderingIngredients` (uses session key `"ingredientsCart"`).
  - `Areas/Waiter/`: `BookingTables`, `Ordering` (uses session key `"shoppingCart"`), and `ChangingOrderStatuses`.

## Seeded Roles, Users & Lookup Data

`UserRoleInitializer.cs` seeds these roles and users on startup:

| Role | Username | Password |
| :--- | :--- | :--- |
| `adminIS` | `adminIS1` | `admin` |
| `adminRes` | `adminRes1` | `admin` |
| `waiter` | `waiter1` | `waiter` |
| `cooker` | `cooker1` | `cooker` |

`SampleData.cs` seeds lookup tables on startup; status IDs are exposed as domain constants on the status models and referenced across controllers and `ApplicationContext`:
- `DishStatus`: `AwaitingId = 1` (`awaiting`), `AcceptedId = 2` (`accepted`), `ReadyId = 3` (`ready`)
- `OrderStatus`: `OpenId = 1` (`open`), `ClosedId = 2` (`close`)
- `OrderIngredientsStatus`: `OpenId = 1` (`open`), `ClosedId = 2` (`close`)
- `Category`, `DishUnit`, `IngredientUnit`

## Important Gotchas & Conventions

- **PostgreSQL connection & timestamps**: `Program.cs` uses `ConnectionStrings:DefaultConnection` from `appsettings.json` (`Host=localhost;Port=5432;Database=dbrestaurant;Username=postgres;Password=postgres`). Domain `DateTime` properties are mapped to `timestamp without time zone` (with `LOCALTIMESTAMP` defaults) so Npgsql 11 accepts `DateTime.Now`/`DateTime.Today` (`Local`) and MVC form-bound dates (`Unspecified`) without timezone conversion errors; Identity's `LockoutEnd` (`DateTimeOffset?`) uses `timestamp with time zone`.
- **Observability & logging**: Prefer source-generated `[LoggerMessage]` methods in `Observability/LogEvents.cs` and instruments in `Observability/RestaurantTelemetry.cs` over ad-hoc `ILogger.Log*` string formatting or `Debug.WriteLine`. Never log passwords, tokens, cookies, or sensitive form payloads. OTLP export is opt-in and only registered when `OpenTelemetry:OtlpEndpoint` or `OTEL_EXPORTER_OTLP_ENDPOINT` is set; EF Core SQL logging is controlled via `Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command` in `appsettings*.json`.
- **Naming conventions**: `UseSnakeCaseNamingConvention()` handles convention-based names; `ApplicationContext` also explicitly maps ASP.NET Core Identity tables (`asp_net_*`) and named indexes (`email_index`, `user_name_index`, `role_name_index`) to `snake_case` because `IdentityDbContext` overrides conventions with explicit PascalCase names.
- **Authorization attributes**: `Cooker` and `Waiter` controllers apply `[Authorize(Roles = "...")]`, whereas `Administration` and `RestaurantAdministration` controllers currently only have `[Area(...)]` without `[Authorize]`. Preserve or tighten carefully depending on task requirements.
- **Session carts**: Both `OrderingController` (`"shoppingCart"`) and `OrderingIngredientsController` (`"ingredientsCart"`) store EF entities in session and clear navigation properties (`sc.Dish = null!` / `ic.Ingredient = null!`) before `context.Add(...)` + `SaveChangesAsync()` to avoid inserting duplicate catalog rows. `ShoppingCartItem.UserId` is nullable because a cook is not assigned when a waiter creates an order.
- **NRT & MVC validation**: `SuppressImplicitRequiredAttributeForNonNullableReferenceTypes` is enabled in `Program.cs` so non-nullable navigation/server-populated properties on bound EF entities (`Order.UserId`, `Dish.Category`, etc.) do not fail form `ModelState` validation. Keep `Models/` nullability aligned with `ApplicationContext` (`.IsRequired()`).

## Git Workflow

This repository uses a `develop`-first promotion flow across four long-lived branches (`develop`, `test`, `staging`, `main`):

1. **Feature branches**: Always branch new work from `develop` using `feature/<short-kebab-name>` (e.g., `feature/postgresql-migration`), and open pull requests targeting **`develop`** — never open feature PRs directly against `main`.
   - Note: In single-branch clones where only `main` is fetched initially, fetch `develop` first (`git fetch origin develop:refs/remotes/origin/develop`) before creating the feature branch.
2. **Environment promotion**: After merging into `develop`, promote `develop` through environment branches via PRs:
   - `develop` → `test` (`Promote develop to test: <summary>`)
   - `develop` → `staging` (`Promote develop to staging: <summary>`)
   - `develop` → `main` (`Promote develop to main: <summary>`)
