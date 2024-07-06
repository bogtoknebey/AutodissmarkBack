using Autodissmark.Core.Constants;
using Microsoft.AspNetCore.Http.Extensions;

namespace Autodissmark.API.Extentions;

internal static class HttpContextExtension
{
    public static string GetUserId(this HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.Claims
            .FirstOrDefault(clam => clam.Type == ClaimConstants.UserId);

        var headers = httpContext.Request.Headers;
        var url = httpContext.Request.GetDisplayUrl();

        return userIdClaim is null
            ? throw new Exception("UserId claim not found.")
            : userIdClaim.Value;
    }

    public static string GetUserRole(this HttpContext httpContext)
    {
        var userRoleClaim = httpContext.User.Claims
            .FirstOrDefault(clam => clam.Type == ClaimConstants.Role);

        var headers = httpContext.Request.Headers;
        var url = httpContext.Request.GetDisplayUrl();

        return userRoleClaim is null
            ? throw new Exception("UserRole claim not found.")
            : userRoleClaim.Value;
    }
}
