using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.UseCases.UserRoles.Commands;

public class UpdateUserRoleUseCase(IUnitOfWork unitOfWork)
{
    public async Task<UserRoleDto> ExecuteAsync(Guid userId, Guid roleId, UpdateUserRoleRequest request)
    {
        var current = await unitOfWork.UserRoles.GetByIdAsync(userId, roleId)
                      ?? throw new NotFoundException("The user does not have that role.");

        if (request.NewRoleId == roleId)
            throw new ConflictException("The new role is the same as the current one.");

        var newRole = await unitOfWork.Roles.GetByIdAsync(request.NewRoleId)
                      ?? throw new NotFoundException($"Role '{request.NewRoleId}' was not found.");

        if (await unitOfWork.UserRoles.GetByIdAsync(userId, newRole.RoleId) is not null)
            throw new ConflictException($"The user already has the role '{newRole.RoleName}'.");

        var replacement = new UserRole { UserId = userId, RoleId = newRole.RoleId, Role = newRole };

        unitOfWork.UserRoles.Remove(current);
        await unitOfWork.UserRoles.AddAsync(replacement);
        await unitOfWork.SaveChangesAsync();

        return UserRoleDto.FromEntity(replacement);
    }
}
