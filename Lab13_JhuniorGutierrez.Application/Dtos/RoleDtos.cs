using System.ComponentModel.DataAnnotations;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.Dtos;

public record RoleDto(Guid RoleId, string RoleName)
{
    public static RoleDto FromEntity(Role role) => new(role.RoleId, role.RoleName);
}

public record CreateRoleRequest([Required, StringLength(50, MinimumLength = 2)] string RoleName);

public record UpdateRoleRequest([Required, StringLength(50, MinimumLength = 2)] string RoleName);

public record UpdateUserRoleRequest([Required] Guid NewRoleId);

public record AssignRoleRequest([Required] Guid UserId, [Required] Guid RoleId);

public record UserRoleDto(Guid UserId, Guid RoleId, string RoleName, DateTime? AssignedAt)
{
    public static UserRoleDto FromEntity(UserRole userRole) =>
        new(userRole.UserId, userRole.RoleId, userRole.Role.RoleName, userRole.AssignedAt);
}
