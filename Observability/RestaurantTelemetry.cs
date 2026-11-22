using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace WebApplicationRestaurant.Observability;

public static class RestaurantTelemetry
{
    public const string ServiceName = "WebApplicationRestaurant";
    public const string ActivitySourceName = ServiceName;
    public const string MeterName = ServiceName;

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName, "1.0.0");
    public static readonly Meter Meter = new(MeterName, "1.0.0");

    public static readonly Counter<long> AuthLogins =
        Meter.CreateCounter<long>(
            "restaurant.auth.logins",
            unit: "{attempt}",
            description: "User login attempts by outcome.");

    public static readonly Counter<long> OrdersCreated =
        Meter.CreateCounter<long>(
            "restaurant.orders.created",
            unit: "{order}",
            description: "Customer orders created.");

    public static readonly Histogram<int> OrderItemsCount =
        Meter.CreateHistogram<int>(
            "restaurant.orders.items_count",
            unit: "{item}",
            description: "Number of items per created customer order.");

    public static readonly Counter<long> OrdersClosed =
        Meter.CreateCounter<long>(
            "restaurant.orders.closed",
            unit: "{order}",
            description: "Customer orders closed.");

    public static readonly Counter<long> DishStatusChanges =
        Meter.CreateCounter<long>(
            "restaurant.dishes.status_changes",
            unit: "{change}",
            description: "Kitchen dish preparation status transitions.");

    public static readonly Counter<long> TableReservations =
        Meter.CreateCounter<long>(
            "restaurant.tables.reservations",
            unit: "{operation}",
            description: "Table booking and unbooking operations.");

    public static readonly Counter<long> IngredientOrdersCreated =
        Meter.CreateCounter<long>(
            "restaurant.ingredient_orders.created",
            unit: "{order}",
            description: "Ingredient procurement orders created.");

    public static readonly Counter<long> IngredientOrdersClosed =
        Meter.CreateCounter<long>(
            "restaurant.ingredient_orders.closed",
            unit: "{order}",
            description: "Ingredient procurement orders closed.");

    public static readonly Counter<long> AdminUserEvents =
        Meter.CreateCounter<long>(
            "restaurant.admin.user_events",
            unit: "{event}",
            description: "User and role management operations.");
}
