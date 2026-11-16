using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;

namespace Lab13_JhuniorGutierrez.Application.UseCases.Reports.Queries;

public class ExportUsersReportUseCase(IUnitOfWork unitOfWork, IExcelReportGenerator excel)
{
    private static readonly string[] Headers =
        [ "Username", "Email", "Roles", "Tickets opened", "Created at"];

    public async Task<ExcelFileDto> ExecuteAsync()
    {
        var users = await unitOfWork.Users.GetAllWithRolesAsync();
        var tickets = await unitOfWork.Tickets.GetAllWithUserAsync();
        var ticketsByUser = tickets.GroupBy(t => t.UserId).ToDictionary(g => g.Key, g => g.Count());

        var rows = users
            
            .Select(u => (IReadOnlyList<object?>)
            [
                u.Username,
                u.Email,
                string.Join(", ", u.UserRoles.Select(ur => ur.Role.RoleName).OrderBy(r => r)),
                ticketsByUser.GetValueOrDefault(u.UserId),
                u.CreatedAt
            ])
            .ToList();

        var content = excel.Generate(new ExcelTable("Users", Headers, rows));
        return new ExcelFileDto(content, $"users-report-{DateTime.UtcNow:yyyyMMdd-HHmmss}.xlsx");
    }
}
