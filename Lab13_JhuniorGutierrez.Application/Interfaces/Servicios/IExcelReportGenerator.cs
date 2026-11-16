using Lab13_JhuniorGutierrez.Application.Dtos;

namespace Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;

/// <summary>Port that renders tabular data as an .xlsx workbook.</summary>
public interface IExcelReportGenerator
{
    byte[] Generate(ExcelTable table);
}
