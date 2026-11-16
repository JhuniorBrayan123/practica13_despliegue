using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Enums;
using Lab13_JhuniorGutierrez.Domain.Exceptions;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Tickets.Commands;

public class ChangeTicketStatusUseCase(IUnitOfWork unitOfWork)
{
    public async Task<TicketDto> ExecuteAsync(Guid ticketId, ChangeTicketStatusRequest request)
    {
        var ticket = await unitOfWork.Tickets.GetByIdWithUserAsync(ticketId)
                     ?? throw new NotFoundException($"Ticket '{ticketId}' was not found.");

        var newStatus = request.Status!.Value;
        if (ticket.Status == newStatus)
            throw new DomainException($"The ticket is already '{newStatus}'.");

        ticket.Status = newStatus;
        ticket.ClosedAt = newStatus == TicketStatus.Cerrado ? DateTime.Now : null;

        unitOfWork.Tickets.Update(ticket);
        await unitOfWork.SaveChangesAsync();

        return TicketDto.FromEntity(ticket);
    }
}
