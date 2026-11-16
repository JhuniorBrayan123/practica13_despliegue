using System.ComponentModel.DataAnnotations;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.Dtos;

public record UserDto(Guid UserId, string Username, string? Email, DateTime? CreatedAt, IReadOnlyList<string> Roles)
{
    public static UserDto FromEntity(User user) => new(
        user.UserId,
        user.Username,
        user.Email,
        user.CreatedAt,
        user.UserRoles.Select(ur => ur.Role.RoleName).OrderBy(r => r).ToList());
}

public record CreateUserRequest(
    [Required, StringLength(100, MinimumLength = 3)] string Username,
    [Required, StringLength(100, MinimumLength = 6)] string Password,
    [EmailAddress, StringLength(150)] string? Email,
    List<Guid>? RoleIds);

public record UpdateUserRequest(
    [Required, StringLength(100, MinimumLength = 3)] string Username,
    [EmailAddress, StringLength(150)] string? Email,
    [StringLength(100, MinimumLength = 6)] string? NewPassword);
