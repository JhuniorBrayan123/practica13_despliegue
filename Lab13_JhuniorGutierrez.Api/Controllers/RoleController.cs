using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.UseCases.Roles.Commands;
using Lab13_JhuniorGutierrez.Application.UseCases.Roles.Queries;
using Lab13_JhuniorGutierrez.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab13_JhuniorGutierrez.Api.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = RoleNames.Admin)]
[Produces("application/json")]
public class RoleController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<RoleDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] GetRolesUseCase useCase) =>
        Ok(await useCase.ExecuteAsync());

    [HttpPost]
    [ProducesResponseType<RoleDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(CreateRoleRequest request, [FromServices] CreateRoleUseCase useCase)
    {
        var role = await useCase.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = role.RoleId }, role);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<RoleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, [FromServices] GetRoleByIdUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(id));

    [HttpPut("{id:guid}")]
    [ProducesResponseType<RoleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, UpdateRoleRequest request, [FromServices] UpdateRoleUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(id, request));

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id, [FromServices] DeleteRoleUseCase useCase)
    {
        await useCase.ExecuteAsync(id);
        return NoContent();
    }
}
