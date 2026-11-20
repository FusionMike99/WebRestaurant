# AGENTS.md

Guidance for AI coding agents working in this repository.

## Project Overview

**WebApplicationRestaurant** is an ASP.NET Core 11 (`net11.0`, C# 15) MVC web application for restaurant operations management (UI text, `[Display]` attributes, and validation messages are in Ukrainian).

- **Framework**: ASP.NET Core MVC 11, Entity Framework Core 11.0.0 (SQL Server), ASP.NET Core Identity
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
```

- **Tests**: There is currently no test project in the repository (`WebApplicationRestaurant.csproj` already excludes `WebApplicationRestaurant.Tests/**` and `AGENTS.md`).
- **EF Core migrations**: Migrations live in `Migrations/` (`InitialCreate`, `Migration2`). `Microsoft.EntityFrameworkCore.Design` is referenced explicitly alongside `Microsoft.EntityFrameworkCore.Tools` (required in EF Core 11).

## IDE & MCP Tooling

- **Prioritize JetBrains Rider MCP** (`jetbrains-rider` configured in `.mcp.json` / `.air/mcp.json`, or `jetbrains_air_ide` via `execute_tool` / `post_edit_quality_check` when running inside Air/Rider):
  - **Code navigation & symbol lookups**: Prefer `search_symbol`, `find_usages`, `find_implementations`, and `get_symbol_info` over text/grep searches.
  - **Refactoring & formatting**: Prefer Rider refactoring tools (`rename_refactoring`, `move_type_to_namespace`, `extract_method`, `reformat_file`) over manual multi-file edits.
  - **Diagnostics & builds**: Prefer `get_file_problems`, `lint_files`, `get_project_problems`, `post_edit_quality_check`, and `build_solution_start` / `build_solution_state` for IDE inspections and builds, falling back to CLI `dotnet build` only if Rider MCP is unavailable or disconnected.
- **Microsoft Learn MCP (`.mcp.json` & `.agents/mcp_config.json`)**: Configured with `microsoft-learn` (`https://learn.microsoft.com/api/mcp`) for up-to-date ASP.NET Core 11, EF Core 11, Identity, and C# 15 documentation and code samples (`microsoft_docs_search`, `microsoft_docs_fetch`, `microsoft_code_sample_search`). `.agents/mcp_config.json` is loaded by Google Antigravity, while `.mcp.json` is loaded by Rider/Air, Claude Code, and Cursor.
- **Repository Skills & Custom Agents**: Curated skills live in `.agents/skills/` (Microsoft Learn docs/reference/CLI, EF Core query optimization, data-driven ASP.NET Core MVC scaffolding, C# refactoring, .NET performance analysis, `System.Text.Json` .NET 11, and .NET testing workflows). Custom agents (`optimizing-dotnet-performance`, `test-engineer`, `msbuild-code-review`) live in `.agents/agents/*.md` (Antigravity-native frontmatter) and `.github/agents/*.agent.md` (GitHub Copilot / IDE format). See `.agents/SOURCES.md` for upstream sources and pinned versions.

## Architecture & Directory Structure

- `Program.cs`: Top-level statements using `WebApplication.CreateBuilder(args)`. Configures `ApplicationContext`, Identity (relaxed password rules: min length 5, no digit/case/symbol requirements), MVC (`SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true` to preserve attribute-driven form validation), `DistributedMemoryCache` + `Session`, `ru-RU` culture with `.` as decimal separator, `MapStaticAssets()`, and MVC routes (`areas` + `default`). Also runs `SampleData.Initialize` and `UserRoleInitializer.InitializeAsync` inside an async DI scope on startup.
- `SessionExtensions.cs`: `ISession.Set<T>` / `Get<T?>` JSON serialization helpers (`System.Text.Json`).
- `Data/ApplicationContext.cs`: `IdentityDbContext<User>` (primary constructor) with Fluent API mapping to explicit table names (`Category`, `Dish`, `DishStatus`, `DishUnit`, `Ingredient`, `IngredientUnit`, `MenuPlan`, `Order`, `OrderIngredients`, `OrderIngredientsItem`, `OrderIngredientsStatus`, `OrderStatus`, `Place`, `Schedule`, `ShoppingCartItem`) and implicit join tables (`Recipe`, `MenuPlanItem`, `SchedulePlace`). Many `Name` columns are configured as fixed-length `nchar(20)` (`IsFixedLength(true)`).
- `Controllers/`:
  - `HomeController.cs`: Redirects unauthenticated users to `Account/Login`; renders role-specific partial links (`Views/Home/*Links.cshtml`). For `waiter`, loads today's `Schedule.Places` into `ViewBag.Places` (falling back to `[]` if none scheduled).
  - `AccountController.cs`: Login and logout actions.
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

`Migration2` and `SampleData.cs` seed lookup tables whose IDs are referenced directly in controllers:
- `DishStatus`: `1` = `awaiting`, `2` = `accepted`, `3` = `ready`
- `OrderStatus`: `1` = `open`, `2` = `close`
- `OrderIngredientsStatus`: `1` = `open`, `2` = `close`
- `Category`, `DishUnit`, `IngredientUnit`

## Important Gotchas & Conventions

- **Connection string & SqlClient 7**: `Program.cs` uses `SqlServerDevelopmentConnection` from `appsettings.json` (`Server=DESKTOP-5RK077N;Database=dbrestaurant;Trusted_Connection=True;`), not `DefaultConnection`. `Microsoft.Data.SqlClient` 7 (via EF Core 11) uses `Encrypt=True` by default and separates Entra ID auth into `Microsoft.Data.SqlClient.Extensions.Azure`; add `TrustServerCertificate=True;` in dev if your SQL Server uses a self-signed certificate.
- **External SQL Server stored procedures**: Two controllers execute stored procedures that are **not** defined in EF Core migrations and must exist in the database:
  - `IsExistMenuPlan @StartDate, @EndDate, @Result OUTPUT` in `Areas/RestaurantAdministration/Controllers/MenuPlansController.cs`
  - `IsOrderComplete @OrderId, @DishStatusId, @Result OUTPUT` in `Areas/Waiter/Controllers/ChangingOrderStatusesController.cs`
- **Authorization attributes**: `Cooker` and `Waiter` controllers apply `[Authorize(Roles = "...")]`, whereas `Administration` and `RestaurantAdministration` controllers currently only have `[Area(...)]` without `[Authorize]`. Preserve or tighten carefully depending on task requirements.
- **Session carts**: Both `OrderingController` (`"shoppingCart"`) and `OrderingIngredientsController` (`"ingredientsCart"`) store EF entities in session and clear navigation properties (`sc.Dish = null!` / `ic.Ingredient = null!`) before `context.Add(...)` + `SaveChangesAsync()` to avoid inserting duplicate catalog rows.
- **NRT & MVC validation**: `SuppressImplicitRequiredAttributeForNonNullableReferenceTypes` is enabled in `Program.cs` so non-nullable navigation/server-populated properties on bound EF entities (`Order.UserId`, `Dish.Category`, etc.) do not fail form `ModelState` validation. Keep `Models/` nullability aligned with `ApplicationContext` (`.IsRequired()`).
