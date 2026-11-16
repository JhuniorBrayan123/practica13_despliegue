using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Constants;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Roles.Commands;

public class UpdateRoleUseCase(IUnitOfWork unitOfWork)
{
    public async Task<RoleDto> ExecuteAsync(Guid roleId, UpdateRoleRequest request)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(roleId)
                   ?? throw new NotFoundException($"Role '{roleId}' was not found.");

        if (RoleNames.IsBuiltIn(role.RoleName))
            throw new ConflictException($"The built-in role '{role.RoleName}' cannot be renamed.");

        var roleName = request.RoleName.Trim();
        var existing = await unitOfWork.Roles.GetByNameAsync(roleName);
        if (existing is not null && existing.RoleId != roleId)
            throw new ConflictException($"The role '{roleName}' already exists.");

        role.RoleName = roleName;
        unitOfWork.Roles.Update(role);
        await unitOfWork.SaveChangesAsync();

        return RoleDto.FromEntity(role);
    }
}
