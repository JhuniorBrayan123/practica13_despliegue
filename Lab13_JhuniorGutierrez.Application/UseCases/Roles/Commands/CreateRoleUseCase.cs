using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Domain.Models;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Roles.Commands;

public class CreateRoleUseCase(IUnitOfWork unitOfWork)
{
    public async Task<RoleDto> ExecuteAsync(CreateRoleRequest request)
    {
        var roleName = request.RoleName.Trim();

        if (await unitOfWork.Roles.GetByNameAsync(roleName) is not null)
            throw new ConflictException($"The role '{roleName}' already exists.");

        var role = new Role { RoleId = Guid.NewGuid(), RoleName = roleName };
        await unitOfWork.Roles.AddAsync(role);
        await unitOfWork.SaveChangesAsync();

        return RoleDto.FromEntity(role);
    }
}
