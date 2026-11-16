namespace Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}
