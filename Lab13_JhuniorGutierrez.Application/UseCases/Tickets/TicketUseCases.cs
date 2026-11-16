using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Models;
    
namespace Lab13_JhuniorGutierrez.Application.UseCases.Tickets;


internal static class TicketAccess
{
    public static async Task<Ticket> GetAuthorizedAsync(IUnitOfWork unitOfWork, Guid ticketId, CurrentUser currentUser)
    {
        var ticket = await unitOfWork.Tickets.GetByIdWithUserAsync(ticketId)
                     ?? throw new NotFoundException($"Ticket '{ticketId}' was not found.");

        if (!currentUser.IsStaff && ticket.UserId != currentUser.UserId)
            throw new ForbiddenException("You can only access your own tickets.");

        return ticket;
    }
}
