using Lab13_JhuniorGutierrez.Application.Common;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;
using Lab13_JhuniorGutierrez.Domain.Enums;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Reports.Queries;


public class ExportTicketsReportUseCase(IUnitOfWork unitOfWork, IExcelReportGenerator excel)
{
    private static readonly string[] Headers =
        [ "Title", "Description", "Status", "Created by", "Created at", "Closed at"];

    public async Task<ExcelFileDto> ExecuteAsync(CurrentUser currentUser, TicketStatus? status = null)
    {
        var tickets = currentUser.IsStaff
            ? await unitOfWork.Tickets.GetAllWithUserAsync(status)
            : await unitOfWork.Tickets.GetByUserAsync(currentUser.UserId, status);

        var rows = tickets
            .Select(t => (IReadOnlyList<object?>)
            [
                t.Title,
                t.Description,
                t.Status.ToString(),
                t.User.Username,
                t.CreatedAt,
                t.ClosedAt
            ])
            .ToList();

        var content = excel.Generate(new ExcelTable("Tickets", Headers, rows));
        return new ExcelFileDto(content, $"tickets-report-{DateTime.UtcNow:yyyyMMdd-HHmmss}.xlsx");
    }
}
