using Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;

namespace Lab13_JhuniorGutierrez.Infrastructure.Security;

public class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string passwordHash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            // Stored value is not a BCrypt hash (e.g. legacy plain-text data).
            return false;
        }
    }
}
