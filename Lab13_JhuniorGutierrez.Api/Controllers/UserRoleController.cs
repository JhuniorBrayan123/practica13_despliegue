using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.UseCases.UserRoles.Commands;
using Lab13_JhuniorGutierrez.Application.UseCases.UserRoles.Queries;
using Lab13_JhuniorGutierrez.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab13_JhuniorGutierrez.Api.Controllers;


[ApiController]
[Route("api/user-roles")]
[Authorize(Roles = RoleNames.Admin)]
[Produces("application/json")]
public class UserRoleController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<UserRoleDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] GetAllUserRolesUseCase useCase) =>
        Ok(await useCase.ExecuteAsync());

    [HttpGet("{userId:guid}")]
    [ProducesResponseType<IReadOnlyList<UserRoleDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, [FromServices] GetUserRolesUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(userId));

    
    [HttpPost]
    [ProducesResponseType<UserRoleDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Assign(AssignRoleRequest request, [FromServices] AssignRoleUseCase useCase)
    {
        var userRole = await useCase.ExecuteAsync(request);
        return StatusCode(StatusCodes.Status201Created, userRole);
    }

    [HttpPut("{userId:guid}/{roleId:guid}")]
    [ProducesResponseType<UserRoleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid userId, Guid roleId, UpdateUserRoleRequest request,
        [FromServices] UpdateUserRoleUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(userId, roleId, request));

    [HttpDelete("{userId:guid}/{roleId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Remove(Guid userId, Guid roleId, [FromServices] RemoveRoleUseCase useCase)
    {
        await useCase.ExecuteAsync(userId, roleId);
        return NoContent();
    }
}
