using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.UseCases.Tickets;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Responses.Queries;

public class GetResponseByIdUseCase(IUnitOfWork unitOfWork)
{
    public async Task<ResponseDto> ExecuteAsync(Guid ticketId, Guid responseId, CurrentUser currentUser)
    {
        await TicketAccess.GetAuthorizedAsync(unitOfWork, ticketId, currentUser);

        var response = await unitOfWork.Responses.GetByIdWithResponderAsync(responseId);
        if (response is null || response.TicketId != ticketId)
            throw new NotFoundException($"Response '{responseId}' was not found in ticket '{ticketId}'.");

        return ResponseDto.FromEntity(response);
    }
}
