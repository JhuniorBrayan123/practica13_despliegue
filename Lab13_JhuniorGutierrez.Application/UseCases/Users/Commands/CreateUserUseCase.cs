using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;
using Lab13_JhuniorGutierrez.Domain.Constants;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Users.Commands;

public class CreateUserUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
{
    public async Task<UserDto> ExecuteAsync(CreateUserRequest request)
    {
        var username = request.Username.Trim();
        var email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();

        if (await unitOfWork.Users.UsernameExistsAsync(username))
            throw new ConflictException($"The username '{username}' is already taken.");

        if (email is not null && await unitOfWork.Users.EmailExistsAsync(email))
            throw new ConflictException($"The email '{email}' is already registered.");

        var roles = new List<Role>();
        if (request.RoleIds is { Count: > 0 })
        {
            foreach (var roleId in request.RoleIds.Distinct())
                roles.Add(await unitOfWork.Roles.GetByIdAsync(roleId)
                          ?? throw new NotFoundException($"Role '{roleId}' was not found."));
        }
        else
        {
            roles.Add(await unitOfWork.Roles.GetByNameAsync(RoleNames.User)
                      ?? throw new NotFoundException($"The default role '{RoleNames.User}' does not exist."));
        }

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = passwordHasher.Hash(request.Password)
        };
        foreach (var role in roles)
            user.UserRoles.Add(new UserRole { UserId = user.UserId, RoleId = role.RoleId, Role = role });

        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        return UserDto.FromEntity(user);
    }
}
