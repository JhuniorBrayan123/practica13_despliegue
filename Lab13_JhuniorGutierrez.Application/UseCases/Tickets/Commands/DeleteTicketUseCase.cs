using System;
using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Tickets.Commands;

public class DeleteTicketUseCase(IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(Guid ticketId)
    {
        var ticket = await unitOfWork.Tickets.GetByIdAsync(ticketId)
                     ?? throw new NotFoundException($"Ticket '{ticketId}' was not found.");

        unitOfWork.Tickets.Remove(ticket);
        await unitOfWork.SaveChangesAsync();
    }
}