using Lab13_JhuniorGutierrez.Api.Extensions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.UseCases.Responses.Commands;
using Lab13_JhuniorGutierrez.Application.UseCases.Responses.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab13_JhuniorGutierrez.Api.Controllers;

[ApiController]
[Route("api/tickets/{ticketId:guid}/responses")]
[Authorize]
[Produces("application/json")]
public class ResponseController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<ResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(Guid ticketId, [FromServices] GetTicketResponsesUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(ticketId, User.ToCurrentUser()));

   
    [HttpPost]
    [ProducesResponseType<ResponseDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(Guid ticketId, CreateResponseRequest request,
        [FromServices] AddResponseUseCase useCase)
    {
        var response = await useCase.ExecuteAsync(ticketId, request, User.ToCurrentUser());
        return CreatedAtAction(nameof(GetById), new { ticketId, responseId = response.ResponseId }, response);
    }

    [HttpGet("{responseId:guid}")]
    [ProducesResponseType<ResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid ticketId, Guid responseId,
        [FromServices] GetResponseByIdUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(ticketId, responseId, User.ToCurrentUser()));

    [HttpPut("{responseId:guid}")]
    [ProducesResponseType<ResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(Guid ticketId, Guid responseId, UpdateResponseRequest request,
        [FromServices] UpdateResponseUseCase useCase) =>
        Ok(await useCase.ExecuteAsync(ticketId, responseId, request, User.ToCurrentUser()));

    [HttpDelete("{responseId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid ticketId, Guid responseId,
        [FromServices] DeleteResponseUseCase useCase)
    {
        await useCase.ExecuteAsync(ticketId, responseId, User.ToCurrentUser());
        return NoContent();
    }
}
