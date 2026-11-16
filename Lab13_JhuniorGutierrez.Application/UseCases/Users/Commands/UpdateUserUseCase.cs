using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Users.Commands;

public class UpdateUserUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
{
    public async Task<UserDto> ExecuteAsync(Guid userId, UpdateUserRequest request, CurrentUser currentUser)
    {
        if (!currentUser.IsStaff && currentUser.UserId != userId)
            throw new ForbiddenException("You can only update your own profile.");

        var user = await unitOfWork.Users.GetByIdWithRolesAsync(userId)
                   ?? throw new NotFoundException($"User '{userId}' was not found.");

        var username = request.Username.Trim();
        var email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();

        if (await unitOfWork.Users.UsernameExistsAsync(username, userId))
            throw new ConflictException($"The username '{username}' is already taken.");

        if (email is not null && await unitOfWork.Users.EmailExistsAsync(email, userId))
            throw new ConflictException($"The email '{email}' is already registered.");

        user.Username = username;
        user.Email = email;
        if (!string.IsNullOrWhiteSpace(request.NewPassword))
            user.PasswordHash = passwordHasher.Hash(request.NewPassword);

        unitOfWork.Users.Update(user);
        await unitOfWork.SaveChangesAsync();

        return UserDto.FromEntity(user);
    }
}
