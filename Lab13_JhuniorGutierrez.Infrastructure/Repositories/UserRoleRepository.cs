using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Models;
using Lab13_JhuniorGutierrez.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab13_JhuniorGutierrez.Infrastructure.Repositories;

public class UserRoleRepository(Lab13_JhuniorGutierrezDbContext context) : GenericRepository<UserRole>(context), IUserRoleRepository
{
    public async Task<IReadOnlyList<UserRole>> GetByUserAsync(Guid userId) =>
        await DbSet.Include(ur => ur.Role)
            .Where(ur => ur.UserId == userId)
            .OrderBy(ur => ur.Role.RoleName)
            .ToListAsync();

    public async Task<IReadOnlyList<UserRole>> GetAllWithRoleAsync() =>
        await DbSet.Include(ur => ur.Role)
            .AsNoTracking()
            .OrderBy(ur => ur.UserId).ThenBy(ur => ur.Role.RoleName)
            .ToListAsync();
}
