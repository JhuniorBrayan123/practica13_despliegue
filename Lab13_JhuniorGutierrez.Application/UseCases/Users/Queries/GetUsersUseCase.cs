using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Users.Queries;

public class GetUsersUseCase(IUnitOfWork unitOfWork)
{
    public async Task<IReadOnlyList<UserDto>> ExecuteAsync()
    {
        var users = await unitOfWork.Users.GetAllWithRolesAsync();
        return users.Select(UserDto.FromEntity).ToList();
    }
}
