using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.UseCases.Tickets;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Responses.Commands;

public class DeleteResponseUseCase(IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(Guid ticketId, Guid responseId, CurrentUser currentUser)
    {
        await TicketAccess.GetAuthorizedAsync(unitOfWork, ticketId, currentUser);

        var response = await unitOfWork.Responses.GetByIdAsync(responseId);
        if (response is null || response.TicketId != ticketId)
            throw new NotFoundException($"Response '{responseId}' was not found in ticket '{ticketId}'.");

        if (!currentUser.IsStaff && response.ResponderId != currentUser.UserId)
            throw new ForbiddenException("You can only delete your own responses.");

        unitOfWork.Responses.Remove(response);
        await unitOfWork.SaveChangesAsync();
    }
}
