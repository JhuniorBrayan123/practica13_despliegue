using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAt) Generate(User user, IEnumerable<string> roles);
}
