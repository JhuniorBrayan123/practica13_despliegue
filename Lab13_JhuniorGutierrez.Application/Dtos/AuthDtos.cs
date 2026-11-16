using System.ComponentModel.DataAnnotations;

namespace Lab13_JhuniorGutierrez.Application.Dtos;

public record RegisterRequest(
    [Required, StringLength(100, MinimumLength = 3)] string Username,
    [Required, StringLength(100, MinimumLength = 6)] string Password,
    [EmailAddress, StringLength(150)] string? Email);

public record LoginRequest(
    [Required] string Username,
    [Required] string Password);

public record AuthResponse(string Token, DateTime ExpiresAt, Guid UserId, string Username, IReadOnlyList<string> Roles);
