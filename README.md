# WebApplicationRestaurant

An ASP.NET Core MVC web application for managing day-to-day restaurant operations: menu and recipe catalogs, staff schedules, table bookings, customer order taking, kitchen dish preparation workflows, and ingredient supply requests.

The application UI and validation messages are localized in Ukrainian.

## Features by Role

The application separates operational workflows into four role-based MVC areas:

- **IS Administrator (`adminIS` — `Areas/Administration`)**
  - Manage dish categories, dishes, and dish recipes (`EditRecipe`)
  - Manage ingredients and measurement units (for dishes and ingredients)
  - Maintain reference statuses for dishes, customer orders, and ingredient orders
  - Update ingredient supply order statuses (`ChangingOrderIngredientsStatuses`)
  - Manage application users and Identity roles
- **Restaurant Administrator (`adminRes` — `Areas/RestaurantAdministration`)**
  - Create and edit date-ranged menu plans and assign dishes (`MenuPlans` / `EditMenuDishes`)
  - Manage waiter shift schedules and assigned tables/places (`Schedules` / `EditSchedulePlaces`)
- **Waiter (`waiter` — `Areas/Waiter`)**
  - View today's assigned tables on the home dashboard
  - Book and manage table reservations (`BookingTables`)
  - Take customer orders using a session-backed shopping cart (`Ordering`)
  - Track kitchen progress and close completed orders (`ChangingOrderStatuses`)
- **Cook (`cooker` — `Areas/Cooker`)**
  - Track incoming ordered dishes and transition preparation statuses (`awaiting` → `accepted` → `ready`) (`ChangingDishStatuses`)
  - Build and submit ingredient procurement orders using a session-backed cart (`OrderingIngredients`)

## Tech Stack

- **Framework:** ASP.NET Core 11 MVC (`net11.0`, C# 15, nullable reference types enabled)
- **Data Access:** Entity Framework Core 11.0.0 with PostgreSQL (`Npgsql.EntityFrameworkCore.PostgreSQL` 11.0.0, `EFCore.NamingConventions` 11.0.0 `snake_case` schema)
- **Authentication & Authorization:** ASP.NET Core Identity (`IdentityDbContext<User>`)
- **Frontend:** Razor Views, Bootstrap 4.6.0 (`libman.json`), jQuery, jQuery Validation, static asset delivery via `MapStaticAssets()`
- **Session State:** `DistributedMemoryCache` + `ISession` JSON extensions (`System.Text.Json`)
- **Observability:** OpenTelemetry (`OpenTelemetry.Extensions.Hosting`, `Npgsql.OpenTelemetry`, OTLP exporter) for structured logs (`[LoggerMessage]`), distributed traces (`ActivitySource`), and metrics (`Meter`)

## Project Structure

```text
WebRestaurant/
├── Areas/
│   ├── Administration/            # Catalog, reference data, users, roles, supply order statuses
│   ├── RestaurantAdministration/  # Menu plans and waiter shift schedules
│   ├── Cooker/                    # Dish prep status updates and ingredient ordering
│   └── Waiter/                    # Table booking, customer ordering, order status updates
├── Controllers/                   # HomeController (role-based landing), AccountController (login/logout)
├── Data/                          # ApplicationContext (EF Core Fluent API configuration for PostgreSQL)
├── Migrations/                    # EF Core PostgreSQL migrations (InitialCreate)
├── Models/                        # Domain entities (Dish, Category, Ingredient, MenuPlan, Order, Place, Schedule, User, ...)
├── Observability/                 # OpenTelemetry setup, [LoggerMessage] events, ActivitySource/Meter, context middleware
├── ViewModels/                    # MVC view models
├── Views/                         # Shared layouts and root views
├── wwwroot/                       # Static assets (CSS, JS, Bootstrap 4.6, jQuery)
├── Program.cs                     # Application startup, DI, Identity, localization, seeding, routing
├── SampleData.cs                  # Lookup table seeding (categories, statuses, units)
├── UserRoleInitializer.cs         # Default roles and demo user seeding
├── SessionExtensions.cs           # Typed JSON session helpers
└── docker-compose.telemetry.yml   # Optional standalone .NET Aspire Dashboard for local OTLP inspection
```

## Getting Started

### Prerequisites

- [.NET 11 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) (15+ recommended)
- [Docker](https://www.docker.com/) (optional, for the local `.NET Aspire Dashboard` telemetry viewer)

### 1. Configure the Database Connection

Update `ConnectionStrings:DefaultConnection` in `appsettings.json` (or override it via `appsettings.Development.json` / user secrets / `ConnectionStrings__DefaultConnection` environment variable) to point to your PostgreSQL instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=dbrestaurant;Username=postgres;Password=postgres"
  }
}
```

### 2. Apply EF Core Migrations

Apply the Entity Framework Core migrations to create the PostgreSQL database and schema:

```bash
dotnet ef database update
```

### 3. Build and Run

```bash
dotnet restore
dotnet build
dotnet run
```

By default, the application listens on:

- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

On startup, `SampleData.Initialize` and `UserRoleInitializer.InitializeAsync` automatically seed lookup tables, roles, and default accounts if they do not already exist.

## Observability (OpenTelemetry)

The application uses **OpenTelemetry** (`WebApplicationRestaurant.Observability`) across all three telemetry signals:

- **Structured Logs:** `Microsoft.Extensions.Logging` with source-generated `[LoggerMessage]` events (`LogEvents`), `SimpleConsole` in `Development`, `JsonConsole` in `Production`, and OpenTelemetry log bridge (`IncludeFormattedMessage`, `IncludeScopes`). Every request enriches `Activity.Current` and `ILogger` scopes with authenticated user metadata (`UserId`, `UserName`, `UserRoles`) and MVC route info (`Area`, `Controller`, `Action`) via `TelemetryContextMiddleware`.
- **Distributed Traces:** Automatic ASP.NET Core, `HttpClient`, and `Npgsql` (PostgreSQL) spans plus custom business spans (`Order.Create`, `Order.Close`, `IngredientOrder.Create`).
- **Metrics:** ASP.NET Core, `HttpClient`, `.NET Runtime`, `Npgsql`, and custom restaurant counters/histograms (`restaurant.auth.logins`, `restaurant.orders.created`, `restaurant.orders.items_count`, `restaurant.orders.closed`, `restaurant.dishes.status_changes`, `restaurant.tables.reservations`, `restaurant.ingredient_orders.created`, `restaurant.ingredient_orders.closed`, `restaurant.admin.user_events`).

### Viewing Telemetry Locally with .NET Aspire Dashboard

OTLP export is **opt-in**: if no endpoint is configured, the app logs to the console without attempting background OTLP connections.

1. Start the standalone Aspire Dashboard:

   ```bash
   docker compose -f docker-compose.telemetry.yml up -d
   ```

2. Run the application with OTLP export enabled (via environment variable or `OpenTelemetry:OtlpEndpoint` in `appsettings.Development.json`):

   ```bash
   OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317 dotnet run
   ```

3. Open the Aspire Dashboard UI at `http://localhost:18888` to inspect **Structured Logs**, **Traces**, and **Metrics**.

## Default Seeded Accounts

| Role | Username | Password |
| :--- | :--- | :--- |
| `adminIS` | `adminIS1` | `admin` |
| `adminRes` | `adminRes1` | `admin` |
| `waiter` | `waiter1` | `waiter` |
| `cooker` | `cooker1` | `cooker` |
