using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Models;
using Lab13_JhuniorGutierrez.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab13_JhuniorGutierrez.Infrastructure.Repositories;

public class ResponseRepository(Lab13_JhuniorGutierrezDbContext context) : GenericRepository<Response>(context), IResponseRepository
{
    public async Task<IReadOnlyList<Response>> GetByTicketAsync(Guid ticketId) =>
        await DbSet.Include(r => r.Responder)
            .Where(r => r.TicketId == ticketId)
            .OrderBy(r => r.CreatedAt)
            .AsNoTracking()
            .ToListAsync();

    public Task<Response?> GetByIdWithResponderAsync(Guid responseId) =>
        DbSet.Include(r => r.Responder).FirstOrDefaultAsync(r => r.ResponseId == responseId);
}
