using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;
using Lab13_JhuniorGutierrez.Domain.Constants;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Auth;

public class RegisterUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator)
{
    public async Task<AuthResponse> ExecuteAsync(RegisterRequest request)
    {
        var username = request.Username.Trim();
        var email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim();

        if (await unitOfWork.Users.UsernameExistsAsync(username))
            throw new ConflictException($"The username '{username}' is already taken.");

        if (email is not null && await unitOfWork.Users.EmailExistsAsync(email))
            throw new ConflictException($"The email '{email}' is already registered.");

        var defaultRole = await unitOfWork.Roles.GetByNameAsync(RoleNames.User)
                          ?? throw new NotFoundException($"The default role '{RoleNames.User}' does not exist.");

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = passwordHasher.Hash(request.Password)
        };
        user.UserRoles.Add(new UserRole { UserId = user.UserId, RoleId = defaultRole.RoleId });

        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var roles = new[] { defaultRole.RoleName };
        var (token, expiresAt) = tokenGenerator.Generate(user, roles);
        return new AuthResponse(token, expiresAt, user.UserId, user.Username, roles);
    }
}
