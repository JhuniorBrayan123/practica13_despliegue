using System;
using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

namespace Lab13_JhuniorGutierrez.Application.UseCases.UserRoles.Queries;

public class GetUserRolesUseCase(IUnitOfWork unitOfWork)
{

    public async Task<IReadOnlyList<UserRoleDto>> ExecuteAsync(Guid userId)
    {
        _ = await unitOfWork.Users.GetByIdAsync(userId)
            ?? throw new NotFoundException($"User '{userId}'was not found. ");
        var userRoles = await unitOfWork.UserRoles.GetByUserAsync(userId);
        return userRoles.Select(UserRoleDto.FromEntity).ToList();
    }
}