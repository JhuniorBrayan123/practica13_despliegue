using Lab13_JhuniorGutierrez.Api.Extensions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.UseCases.Tickets.Commands;
using Lab13_JhuniorGutierrez.Application.UseCases.Tickets.Queries;
using Lab13_JhuniorGutierrez.Domain.Constants;
using Lab13_JhuniorGutierrez.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab13_JhuniorGutierrez.Api.Controllers;

[ApiController]
[Route("api/tickets")]
[Authorize]
[Produces("application/json")]
public class TicketController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<TicketDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] TicketStatus? status, [FromServices] GetTicketsUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(User.ToCurrentUser(), status));

    [HttpGet("{id:guid}")]
    [ProducesResponseType<TicketDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, [FromServices] GetTicketByIdUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(id, User.ToCurrentUser()));

    [HttpPost]
    [ProducesResponseType<TicketDto>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateTicketRequest request, [FromServices] CreateTicketUseCase useCase)
    {
        var ticket = await useCase.ExecuteAsync(request, User.ToCurrentUser());
        return CreatedAtAction(nameof(GetById), new { id = ticket.TicketId }, ticket);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<TicketDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(Guid id, UpdateTicketRequest request, [FromServices] UpdateTicketUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(id, request, User.ToCurrentUser()));

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = RoleNames.Staff)]
    [ProducesResponseType<TicketDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeStatus(Guid id, ChangeTicketStatusRequest request,
        [FromServices] ChangeTicketStatusUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(id, request));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = RoleNames.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, [FromServices] DeleteTicketUseCase useCase)
    {
        await useCase.ExecuteAsync(id);
        return NoContent();
    }
}
