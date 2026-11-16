using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;
using Lab13_JhuniorGutierrez.Domain.Models;
using Lab13_JhuniorGutierrez.Infrastructure.Configuration;
using Lab13_JhuniorGutierrez.Infrastructure.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Lab13_JhuniorGutierrez.Infrastructure.Services;

public class JwtTokenGenerator(IOptions<JwtSettings> options) : IJwtTokenGenerator
{
    private readonly JwtSettings _settings = options.Value;

    public (string Token, DateTime ExpiresAt) Generate(User user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Username)
        };
        if (!string.IsNullOrEmpty(user.Email))
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
