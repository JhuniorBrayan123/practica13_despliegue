using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Enums;
using Lab13_JhuniorGutierrez.Domain.Exceptions;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Tickets.Commands;

public class UpdateTicketUseCase(IUnitOfWork unitOfWork)
{
    public async Task<TicketDto> ExecuteAsync(Guid ticketId, UpdateTicketRequest request, CurrentUser currentUser)
    {
        var ticket = await TicketAccess.GetAuthorizedAsync(unitOfWork, ticketId, currentUser);

        if (ticket.Status == TicketStatus.Cerrado)
            throw new DomainException("A closed ticket cannot be edited.");

        ticket.Title = request.Title.Trim();
        ticket.Description = request.Description?.Trim();

        unitOfWork.Tickets.Update(ticket);
        await unitOfWork.SaveChangesAsync();

        return TicketDto.FromEntity(ticket);
    }
}
