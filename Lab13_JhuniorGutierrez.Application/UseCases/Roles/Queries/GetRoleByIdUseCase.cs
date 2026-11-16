using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Roles.Queries;

public class GetRoleByIdUseCase(IUnitOfWork unitOfWork)
{
    public async Task<RoleDto> ExecuteAsync(Guid roleId)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(roleId)
                   ?? throw new NotFoundException($"Role '{roleId}' was not found.");

        return RoleDto.FromEntity(role);
    }
}
