using ClosedXML.Excel;
using Lab13_JhuniorGutierrez.Application.Dtos;
using Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;

namespace Lab13_JhuniorGutierrez.Infrastructure.Services;

public class ClosedXmlReportGenerator : IExcelReportGenerator
{
    public byte[] Generate(ExcelTable table)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(table.SheetName);

        for (var col = 0; col < table.Headers.Count; col++)
            worksheet.Cell(1, col + 1).Value = table.Headers[col];

        for (var row = 0; row < table.Rows.Count; row++)
        {
            for (var col = 0; col < table.Headers.Count; col++)
                SetCell(worksheet.Cell(row + 2, col + 1), table.Rows[row][col]);
        }

        var range = worksheet.Range(1, 1, table.Rows.Count + 1, table.Headers.Count);
        range.CreateTable();

        var headerRow = worksheet.Row(1);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.Cyan;
        headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        worksheet.SheetView.FreezeRows(1);
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private static void SetCell(IXLCell cell, object? value)
    {
        switch (value)
        {
            case null:
                break;
            case DateTime date:
                cell.Value = date;
                cell.Style.DateFormat.Format = "yyyy-MM-dd HH:mm";
                break;
            case int number:
                cell.Value = number;
                break;
            default:
                cell.Value = value.ToString();
                break;
        }
    }
}
