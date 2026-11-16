namespace Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;


public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IUserRoleRepository UserRoles { get; }
    ITicketRepository Tickets { get; }
    IResponseRepository Responses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
