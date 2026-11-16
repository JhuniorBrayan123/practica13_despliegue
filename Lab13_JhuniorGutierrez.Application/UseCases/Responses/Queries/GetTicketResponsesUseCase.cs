using System;
using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.UseCases.Tickets;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Responses.Queries;

public class GetTicketResponsesUseCase(IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<ResponseDto>> ExecuteAsync(Guid ticketId, CurrentUser currentUser)
    {
        await TicketAccess.GetAuthorizedAsync(unitOfWork, ticketId, currentUser);

        var responses = await unitOfWork.Responses.GetByTicketAsync(ticketId);
        return responses.Select(ResponseDto.FromEntity).ToList();
    }
}