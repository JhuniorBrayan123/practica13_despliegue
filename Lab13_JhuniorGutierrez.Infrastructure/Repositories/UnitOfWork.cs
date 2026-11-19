using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Infrastructure.Data;

namespace Lab13_JhuniorGutierrez.Infrastructure.Repositories;

public class UnitOfWork(Lab13JhuniorGutierrezDbContext context) : IUnitOfWork
{
    private IUserRepository? _users;
    private IRoleRepository? _roles;
    private IUserRoleRepository? _userRoles;
    private ITicketRepository? _tickets;
    private IResponseRepository? _responses;

    public IUserRepository Users => _users ??= new UserRepository(context);
    public IRoleRepository Roles => _roles ??= new RoleRepository(context);
    public IUserRoleRepository UserRoles => _userRoles ??= new UserRoleRepository(context);
    public ITicketRepository Tickets => _tickets ??= new TicketRepository(context);
    public IResponseRepository Responses => _responses ??= new ResponseRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}
