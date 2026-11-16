using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Models;
using Lab13_JhuniorGutierrez.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab13_JhuniorGutierrez.Infrastructure.Repositories;

public class UserRepository(Lab13_JhuniorGutierrezDbContext context) : GenericRepository<User>(context), IUserRepository
{
    private IQueryable<User> WithRoles() => DbSet.Include(u => u.UserRoles).ThenInclude(ur => ur.Role);

    public Task<User?> GetByUsernameWithRolesAsync(string username) =>
        WithRoles().FirstOrDefaultAsync(u => u.Username == username);

    public Task<User?> GetByIdWithRolesAsync(Guid userId) =>
        WithRoles().FirstOrDefaultAsync(u => u.UserId == userId);

    public async Task<IReadOnlyList<User>> GetAllWithRolesAsync() =>
        await WithRoles().AsNoTracking().OrderBy(u => u.Username).ToListAsync();

    public Task<bool> UsernameExistsAsync(string username, Guid? excludeUserId = null) =>
        DbSet.AnyAsync(u => u.Username == username && u.UserId != excludeUserId);

    public Task<bool> EmailExistsAsync(string email, Guid? excludeUserId = null) =>
        DbSet.AnyAsync(u => u.Email == email && u.UserId != excludeUserId);
}
