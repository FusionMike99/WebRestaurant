using System.Diagnostics;
using System.Security.Claims;

namespace WebApplicationRestaurant.Observability;

public class TelemetryContextMiddleware(RequestDelegate next, ILogger<TelemetryContextMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var activity = Activity.Current;
        var routeValues = httpContext.GetRouteData().Values;

        var area = routeValues.TryGetValue("area", out var areaObj) ? areaObj?.ToString() : null;
        var controller = routeValues.TryGetValue("controller", out var ctrlObj) ? ctrlObj?.ToString() : null;
        var action = routeValues.TryGetValue("action", out var actionObj) ? actionObj?.ToString() : null;

        if (!string.IsNullOrEmpty(area))
        {
            activity?.SetTag("mvc.area", area);
        }

        if (!string.IsNullOrEmpty(controller))
        {
            activity?.SetTag("mvc.controller", controller);
        }

        if (!string.IsNullOrEmpty(action))
        {
            activity?.SetTag("mvc.action", action);
        }

        var scopeValues = new Dictionary<string, object?>
        {
            ["RequestId"] = httpContext.TraceIdentifier
        };

        if (!string.IsNullOrEmpty(area))
        {
            scopeValues["Area"] = area;
        }

        if (!string.IsNullOrEmpty(controller))
        {
            scopeValues["Controller"] = controller;
        }

        if (!string.IsNullOrEmpty(action))
        {
            scopeValues["Action"] = action;
        }

        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = httpContext.User.Identity.Name;
            var roles = string.Join(",", httpContext.User.FindAll(ClaimTypes.Role).Select(c => c.Value));

            if (!string.IsNullOrEmpty(userId))
            {
                activity?.SetTag("enduser.id", userId);
                scopeValues["UserId"] = userId;
            }

            if (!string.IsNullOrEmpty(userName))
            {
                activity?.SetTag("enduser.name", userName);
                scopeValues["UserName"] = userName;
            }

            if (!string.IsNullOrEmpty(roles))
            {
                activity?.SetTag("enduser.roles", roles);
                scopeValues["UserRoles"] = roles;
            }
        }

        using (logger.BeginScope(scopeValues))
        {
            await next(httpContext);
        }
    }
}
