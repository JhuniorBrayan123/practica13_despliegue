using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Models;
using Lab13_JhuniorGutierrez.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab13_JhuniorGutierrez.Infrastructure.Repositories;

public class RoleRepository(Lab13JhuniorGutierrezDbContext context) : GenericRepository<Role>(context), IRoleRepository
{
    public Task<Role?> GetByNameAsync(string roleName) =>
        DbSet.FirstOrDefaultAsync(r => r.RoleName == roleName);

    public Task<bool> IsInUseAsync(Guid roleId) =>
        Context.UserRoles.AnyAsync(ur => ur.RoleId == roleId);
}
