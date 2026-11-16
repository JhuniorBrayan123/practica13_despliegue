using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

public interface IResponseRepository : IGenericRepository<Response>
{
    Task<IReadOnlyList<Response>> GetByTicketAsync(Guid ticketId);
    Task<Response?> GetByIdWithResponderAsync(Guid responseId);
}
