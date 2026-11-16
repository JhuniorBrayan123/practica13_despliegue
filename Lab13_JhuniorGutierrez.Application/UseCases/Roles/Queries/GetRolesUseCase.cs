using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Roles.Queries;

public class GetRolesUseCase(IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<RoleDto>> ExecuteAsync()
    {
        var roles = await unitOfWork.Roles.GetAllAsync();
        return roles.OrderBy(r => r.RoleName).Select(RoleDto.FromEntity).ToList();
    }
}
