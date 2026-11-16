using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Constants;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Roles.Commands;

public class DeleteRoleUseCase(IUnitOfWork unitOfWork)
{
    public async Task ExecuteAsync(Guid roleId)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(roleId)
                   ?? throw new NotFoundException($"Role '{roleId}' was not found.");

        if (RoleNames.IsBuiltIn(role.RoleName))
            throw new ConflictException($"The built-in role '{role.RoleName}' cannot be deleted.");

        if (await unitOfWork.Roles.IsInUseAsync(roleId))
            throw new ConflictException($"The role '{role.RoleName}' is assigned to users and cannot be deleted.");

        unitOfWork.Roles.Remove(role);
        await unitOfWork.SaveChangesAsync();
    }
}
