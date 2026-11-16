using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.UseCases.Tickets;
using Lab13_JhuniorGutierrez.Domain.Enums;
using Lab13_JhuniorGutierrez.Domain.Exceptions;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Responses.Commands;

public class UpdateResponseUseCase(IUnitOfWork unitOfWork)
{
    public async Task<ResponseDto> ExecuteAsync(Guid ticketId, Guid responseId, UpdateResponseRequest request,
        CurrentUser currentUser)
    {
        var ticket = await TicketAccess.GetAuthorizedAsync(unitOfWork, ticketId, currentUser);

        var response = await unitOfWork.Responses.GetByIdWithResponderAsync(responseId);
        if (response is null || response.TicketId != ticketId)
            throw new NotFoundException($"Response '{responseId}' was not found in ticket '{ticketId}'.");

        if (response.ResponderId != currentUser.UserId)
            throw new ForbiddenException("You can only edit your own responses.");

        if (ticket.Status == TicketStatus.Cerrado)
            throw new DomainException("Responses of a closed ticket cannot be edited.");

        response.Message = request.Message.Trim();
        unitOfWork.Responses.Update(response);
        await unitOfWork.SaveChangesAsync();

        return ResponseDto.FromEntity(response);
    }
}
