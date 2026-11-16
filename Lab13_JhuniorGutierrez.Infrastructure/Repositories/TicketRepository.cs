using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Enums;
using Lab13_JhuniorGutierrez.Domain.Models;
using Lab13_JhuniorGutierrez.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab13_JhuniorGutierrez.Infrastructure.Repositories;

public class TicketRepository(Lab13_JhuniorGutierrezDbContext context) : GenericRepository<Ticket>(context), ITicketRepository
{
    private IQueryable<Ticket> Filtered(TicketStatus? status) =>
        DbSet.Include(t => t.User)
            .Where(t => status == null || t.Status == status)
            .OrderByDescending(t => t.CreatedAt);

    public async Task<IReadOnlyList<Ticket>> GetAllWithUserAsync(TicketStatus? status = null) =>
        await Filtered(status).AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<Ticket>> GetByUserAsync(Guid userId, TicketStatus? status = null) =>
        await Filtered(status).Where(t => t.UserId == userId).AsNoTracking().ToListAsync();

    public Task<Ticket?> GetByIdWithUserAsync(Guid ticketId) =>
        DbSet.Include(t => t.User).FirstOrDefaultAsync(t => t.TicketId == ticketId);
}
