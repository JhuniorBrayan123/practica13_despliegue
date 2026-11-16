using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Enums;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Tickets.Commands;

public class CreateTicketUseCase(IUnitOfWork unitOfWork)
{
    public async Task<TicketDto> ExecuteAsync(
        CreateTicketRequest request,
        CurrentUser currentUser)
    {
        var ticket = new Ticket
        {
            TicketId = Guid.NewGuid(),
            UserId = currentUser.UserId,
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            Status = TicketStatus.Abierto
        };

        await unitOfWork.Tickets.AddAsync(ticket);
        await unitOfWork.SaveChangesAsync();

        var created = await unitOfWork.Tickets
            .GetByIdWithUserAsync(ticket.TicketId);

        return TicketDto.FromEntity(created!);
    }
}
