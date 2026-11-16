using System;
using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.UseCases.UserRoles.Commands;

public class AssignRoleUseCase(IUnitOfWork unitOfWork)
{
    public async Task<UserRoleDto> ExecuteAsync(AssignRoleRequest request)
    {
        _ = await unitOfWork.Users.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User '{request.UserId}' was not found.");

        var role = await unitOfWork.Roles.GetByIdAsync(request.RoleId)
                   ?? throw new NotFoundException($"Role '{request.RoleId}' was not found.");

        if (await unitOfWork.UserRoles.GetByIdAsync(request.UserId, request.RoleId) is not null)
            throw new ConflictException($"The user already has the role '{role.RoleName}'.");

        var userRole = new UserRole { UserId = request.UserId, RoleId = role.RoleId, Role = role };
        await unitOfWork.UserRoles.AddAsync(userRole);
        await unitOfWork.SaveChangesAsync();

        return UserRoleDto.FromEntity(userRole);
    }
}