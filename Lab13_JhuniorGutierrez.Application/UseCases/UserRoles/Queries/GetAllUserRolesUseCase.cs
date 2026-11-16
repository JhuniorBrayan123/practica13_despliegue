using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

namespace Lab13_JhuniorGutierrez.Application.UseCases.UserRoles.Queries;

public class GetAllUserRolesUseCase(IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<UserRoleDto>> ExecuteAsync()
    {
        var userRoles = await unitOfWork.UserRoles.GetAllWithRoleAsync();
        return userRoles.Select(UserRoleDto.FromEntity).ToList();
    }
}
