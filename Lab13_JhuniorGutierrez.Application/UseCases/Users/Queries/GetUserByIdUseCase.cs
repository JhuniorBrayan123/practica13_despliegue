using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Users.Queries;

public class GetUserByIdUseCase(IUnitOfWork unitOfWork)
{
    public async Task<UserDto> ExecuteAsync(Guid userId, CurrentUser currentUser)
    {
        if (!currentUser.IsStaff && currentUser.UserId != userId)
            throw new ForbiddenException("You can only view your own profile.");

        var user = await unitOfWork.Users.GetByIdWithRolesAsync(userId)
                   ?? throw new NotFoundException($"User '{userId}' was not found.");

        return UserDto.FromEntity(user);
    }
}
