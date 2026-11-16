using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    Task<IReadOnlyList<UserRole>> GetByUserAsync(Guid userId);
    Task<IReadOnlyList<UserRole>> GetAllWithRoleAsync();
}
