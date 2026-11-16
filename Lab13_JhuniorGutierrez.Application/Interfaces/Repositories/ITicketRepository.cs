using Lab13_JhuniorGutierrez.Domain.Enums;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

public interface ITicketRepository : IGenericRepository<Ticket>
{
    Task<IReadOnlyList<Ticket>> GetAllWithUserAsync(TicketStatus? status = null);
    Task<IReadOnlyList<Ticket>> GetByUserAsync(Guid userId, TicketStatus? status = null);
    Task<Ticket?> GetByIdWithUserAsync(Guid ticketId);
}
