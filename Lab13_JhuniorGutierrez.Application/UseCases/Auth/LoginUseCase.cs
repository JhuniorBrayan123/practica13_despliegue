using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Auth;

public class LoginUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IJwtTokenGenerator tokenGenerator)
{
    public async Task<AuthResponse> ExecuteAsync(LoginRequest request)
    {
        var user = await unitOfWork.Users.GetByUsernameWithRolesAsync(request.Username.Trim());

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid username or password.");

        var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();
        var (token, expiresAt) = tokenGenerator.Generate(user, roles);
        return new AuthResponse(token, expiresAt, user.UserId, user.Username, roles);
    }
}
