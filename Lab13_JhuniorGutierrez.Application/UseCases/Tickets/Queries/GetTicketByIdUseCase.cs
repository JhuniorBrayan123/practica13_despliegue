using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.UseCases.Tickets;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Tickets.Queries;

public class GetTicketByIdUseCase(IUnitOfWork unitOfWork)
{
    public async Task<TicketDto> ExecuteAsync(
        Guid ticketId,
        CurrentUser currentUser)
    {
        var ticket = await TicketAccess.GetAuthorizedAsync(
            unitOfWork,
            ticketId,
            currentUser);

        return TicketDto.FromEntity(ticket);
    }
}