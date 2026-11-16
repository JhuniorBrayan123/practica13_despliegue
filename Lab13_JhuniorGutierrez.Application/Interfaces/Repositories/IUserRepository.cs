using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameWithRolesAsync(string username);
    Task<User?> GetByIdWithRolesAsync(Guid userId);
    Task<IReadOnlyList<User>> GetAllWithRolesAsync();
    Task<bool> UsernameExistsAsync(string username, Guid? excludeUserId = null);
    Task<bool> EmailExistsAsync(string email, Guid? excludeUserId = null);
}
