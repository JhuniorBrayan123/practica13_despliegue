using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Users.Commands;

public class DeleteUserUseCase(IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(Guid userId, CurrentUser currentUser)
    {
        if (currentUser.UserId == userId)
            throw new ConflictException("You cannot delete your own account.");

        var user = await unitOfWork.Users.GetByIdAsync(userId)
                   ?? throw new NotFoundException($"User '{userId}' was not found.");

        var tickets = await unitOfWork.Tickets.GetByUserAsync(userId);
        if (tickets.Count > 0)
            throw new ConflictException("The user still has tickets and cannot be deleted.");

        unitOfWork.Users.Remove(user);
        await unitOfWork.SaveChangesAsync();
    }
}
