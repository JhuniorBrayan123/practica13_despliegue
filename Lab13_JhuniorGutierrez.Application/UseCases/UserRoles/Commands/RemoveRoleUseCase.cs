using System;
using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

namespace Lab13_JhuniorGutierrez.Application.UseCases.UserRoles.Commands;

public class RemoveRoleUseCase(IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(Guid userId, Guid roleId)
    {
        var userRole = await unitOfWork.UserRoles.GetByIdAsync(userId, roleId)
                       ?? throw new NotFoundException("The user does not have that role.");

        var userRoles = await unitOfWork.UserRoles.GetByUserAsync(userId);
        if (userRoles.Count <= 1)
            throw new ConflictException("A user must keep at least one role.");

        unitOfWork.UserRoles.Remove(userRole);
        await unitOfWork.SaveChangesAsync();
    }
}