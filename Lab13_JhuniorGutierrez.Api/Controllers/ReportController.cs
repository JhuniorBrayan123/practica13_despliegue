using Lab13_JhuniorGutierrez.Api.Extensions;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.UseCases.Reports.Queries;
using Lab13_JhuniorGutierrez.Domain.Constants;
using Lab13_JhuniorGutierrez.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab13_JhuniorGutierrez.Api.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportController : ControllerBase
{
    
    [HttpGet("tickets")]
    [Produces(ExcelFileDto.ContentType)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ExportTickets([FromQuery] TicketStatus? status,
        [FromServices] ExportTicketsReportUseCase useCase)
    {
        var file = await useCase.ExecuteAsync(User.ToCurrentUser(), status);
        return File(file.Content, ExcelFileDto.ContentType, file.FileName);
    }
    
    [HttpGet("users")]
    [Authorize(Roles = RoleNames.Staff)]
    [Produces(ExcelFileDto.ContentType)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ExportUsers([FromServices] ExportUsersReportUseCase useCase)
    {
        var file = await useCase.ExecuteAsync();
        return File(file.Content, ExcelFileDto.ContentType, file.FileName);
    }
}
