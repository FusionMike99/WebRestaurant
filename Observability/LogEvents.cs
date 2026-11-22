using Microsoft.Extensions.Logging;

namespace WebApplicationRestaurant.Observability;

public static partial class LogEvents
{
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Database initialization and seeding completed.")]
    public static partial void DatabaseSeedCompleted(this ILogger logger);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Error,
        Message = "An error occurred while seeding the database.")]
    public static partial void DatabaseSeedFailed(this ILogger logger, Exception exception);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Error,
        Message = "Unhandled exception processing request at {Path} (RequestId: {RequestId}).")]
    public static partial void UnhandledException(this ILogger logger, Exception exception, string path, string requestId);

    [LoggerMessage(
        EventId = 2001,
        Level = LogLevel.Information,
        Message = "User {UserName} logged in successfully.")]
    public static partial void UserLoginSucceeded(this ILogger logger, string userName);

    [LoggerMessage(
        EventId = 2002,
        Level = LogLevel.Warning,
        Message = "Failed login attempt for user {UserName} (LockedOut: {IsLockedOut}, NotAllowed: {IsNotAllowed}).")]
    public static partial void UserLoginFailed(this ILogger logger, string userName, bool isLockedOut, bool isNotAllowed);

    [LoggerMessage(
        EventId = 2003,
        Level = LogLevel.Information,
        Message = "User {UserName} logged out.")]
    public static partial void UserLoggedOut(this ILogger logger, string userName);

    [LoggerMessage(
        EventId = 2010,
        Level = LogLevel.Information,
        Message = "Created user {TargetUserId} ({Email}).")]
    public static partial void UserCreated(this ILogger logger, string targetUserId, string email);

    [LoggerMessage(
        EventId = 2011,
        Level = LogLevel.Warning,
        Message = "Failed to create user {Email}: {Errors}.")]
    public static partial void UserCreateFailed(this ILogger logger, string email, string errors);

    [LoggerMessage(
        EventId = 2012,
        Level = LogLevel.Information,
        Message = "Updated user {TargetUserId} ({Email}).")]
    public static partial void UserUpdated(this ILogger logger, string targetUserId, string email);

    [LoggerMessage(
        EventId = 2013,
        Level = LogLevel.Warning,
        Message = "Failed to update user {TargetUserId}: {Errors}.")]
    public static partial void UserUpdateFailed(this ILogger logger, string targetUserId, string errors);

    [LoggerMessage(
        EventId = 2014,
        Level = LogLevel.Information,
        Message = "Deleted user {TargetUserId}.")]
    public static partial void UserDeleted(this ILogger logger, string targetUserId);

    [LoggerMessage(
        EventId = 2015,
        Level = LogLevel.Warning,
        Message = "Failed to delete user {TargetUserId}: {Errors}.")]
    public static partial void UserDeleteFailed(this ILogger logger, string targetUserId, string errors);

    [LoggerMessage(
        EventId = 2016,
        Level = LogLevel.Information,
        Message = "Changed password for user {TargetUserId}.")]
    public static partial void UserPasswordChanged(this ILogger logger, string targetUserId);

    [LoggerMessage(
        EventId = 2017,
        Level = LogLevel.Warning,
        Message = "Failed to change password for user {TargetUserId}: {Errors}.")]
    public static partial void UserPasswordChangeFailed(this ILogger logger, string targetUserId, string errors);

    [LoggerMessage(
        EventId = 2020,
        Level = LogLevel.Information,
        Message = "Created role {RoleName}.")]
    public static partial void RoleCreated(this ILogger logger, string roleName);

    [LoggerMessage(
        EventId = 2021,
        Level = LogLevel.Warning,
        Message = "Failed to create role {RoleName}: {Errors}.")]
    public static partial void RoleCreateFailed(this ILogger logger, string roleName, string errors);

    [LoggerMessage(
        EventId = 2022,
        Level = LogLevel.Information,
        Message = "Deleted role {RoleId} ({RoleName}).")]
    public static partial void RoleDeleted(this ILogger logger, string roleId, string roleName);

    [LoggerMessage(
        EventId = 2023,
        Level = LogLevel.Information,
        Message = "Updated roles for user {TargetUserId} (Added: [{AddedRoles}], Removed: [{RemovedRoles}]).")]
    public static partial void UserRolesUpdated(this ILogger logger, string targetUserId, string addedRoles, string removedRoles);

    [LoggerMessage(
        EventId = 3001,
        Level = LogLevel.Information,
        Message = "Created customer order {OrderId} for place {PlaceId} with {ItemsCount} items by waiter {WaiterUserId}.")]
    public static partial void OrderCreated(this ILogger logger, int orderId, int placeId, int itemsCount, string waiterUserId);

    [LoggerMessage(
        EventId = 3002,
        Level = LogLevel.Warning,
        Message = "Validation failed when creating customer order for place {PlaceId} with {ItemsCount} items by waiter {WaiterUserId}.")]
    public static partial void OrderCreateValidationFailed(this ILogger logger, int placeId, int itemsCount, string waiterUserId);

    [LoggerMessage(
        EventId = 3003,
        Level = LogLevel.Information,
        Message = "Closed customer order {OrderId} for place {PlaceId}.")]
    public static partial void OrderClosed(this ILogger logger, int orderId, int placeId);

    [LoggerMessage(
        EventId = 3004,
        Level = LogLevel.Warning,
        Message = "Rejected closing customer order {OrderId}: {UnreadyCount} dishes are not ready.")]
    public static partial void OrderCloseRejectedUnreadyDishes(this ILogger logger, int orderId, int unreadyCount);

    [LoggerMessage(
        EventId = 3005,
        Level = LogLevel.Warning,
        Message = "Customer order {OrderId} was not found.")]
    public static partial void OrderNotFound(this ILogger logger, int? orderId);

    [LoggerMessage(
        EventId = 3010,
        Level = LogLevel.Information,
        Message = "Changed dish {DishId} status to {StatusId} in order {OrderId}.")]
    public static partial void DishStatusChanged(this ILogger logger, int orderId, int dishId, int statusId);

    [LoggerMessage(
        EventId = 3011,
        Level = LogLevel.Warning,
        Message = "Order item not found when changing dish status (OrderId: {OrderId}, DishId: {DishId}).")]
    public static partial void DishStatusItemNotFound(this ILogger logger, int? orderId, int? dishId);

    [LoggerMessage(
        EventId = 4001,
        Level = LogLevel.Information,
        Message = "Booked table {TableNumber} (PlaceId: {PlaceId}, ReserveTime: {ReserveTime}).")]
    public static partial void TableBooked(this ILogger logger, int placeId, int tableNumber, DateTime? reserveTime);

    [LoggerMessage(
        EventId = 4002,
        Level = LogLevel.Information,
        Message = "Released table {TableNumber} (PlaceId: {PlaceId}).")]
    public static partial void TableUnbooked(this ILogger logger, int placeId, int tableNumber);

    [LoggerMessage(
        EventId = 5001,
        Level = LogLevel.Information,
        Message = "Created ingredient order {OrderId} with {ItemsCount} items by user {CookerUserId}.")]
    public static partial void IngredientOrderCreated(this ILogger logger, int orderId, int itemsCount, string cookerUserId);

    [LoggerMessage(
        EventId = 5002,
        Level = LogLevel.Warning,
        Message = "Validation failed when creating ingredient order with {ItemsCount} items by user {CookerUserId}.")]
    public static partial void IngredientOrderCreateValidationFailed(this ILogger logger, int itemsCount, string cookerUserId);

    [LoggerMessage(
        EventId = 5003,
        Level = LogLevel.Information,
        Message = "Closed ingredient order {OrderId}.")]
    public static partial void IngredientOrderClosed(this ILogger logger, int orderId);

    [LoggerMessage(
        EventId = 5004,
        Level = LogLevel.Warning,
        Message = "Ingredient order {OrderId} was not found.")]
    public static partial void IngredientOrderNotFound(this ILogger logger, int? orderId);
}
