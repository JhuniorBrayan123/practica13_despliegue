using Lab13_JhuniorGutierrez.Api.Extensions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.UseCases.Users.Commands;
using Lab13_JhuniorGutierrez.Application.UseCases.Users.Queries;
using Lab13_JhuniorGutierrez.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab13_JhuniorGutierrez.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
[Produces("application/json")]
public class UserController : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = RoleNames.Staff)]
    [ProducesResponseType<IReadOnlyList<UserDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] GetUsersUseCase useCase) =>
        Ok(await useCase.ExecuteAsync());

    [HttpGet("me")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMe([FromServices] GetUserByIdUseCase useCase)
    {
        var currentUser = User.ToCurrentUser();
        return Ok(await useCase.ExecuteAsync(currentUser.UserId, currentUser));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, [FromServices] GetUserByIdUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(id, User.ToCurrentUser()));

    [HttpPost]
    [Authorize(Roles = RoleNames.Admin)]
    [ProducesResponseType<UserDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(CreateUserRequest request, [FromServices] CreateUserUseCase useCase)
    {
        var user = await useCase.ExecuteAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest request, [FromServices] UpdateUserUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(id, request, User.ToCurrentUser()));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = RoleNames.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id, [FromServices] DeleteUserUseCase useCase)
    {
        await useCase.ExecuteAsync(id, User.ToCurrentUser());
        return NoContent();
    }
}
