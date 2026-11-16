using System.Security.Claims;
using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Domain.Constants;

namespace Lab13_JhuniorGutierrez.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static CurrentUser ToCurrentUser(this ClaimsPrincipal principal)
    {
        var userId = Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isStaff = principal.IsInRole(RoleNames.Admin) || principal.IsInRole(RoleNames.Support);
        return new CurrentUser(userId, isStaff);
    }
}
