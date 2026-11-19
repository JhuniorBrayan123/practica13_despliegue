using Lab13_JhuniorGutierrez.Application.Dtos;

namespace Lab13_JhuniorGutierrez.Application.Interfaces.Servicios;

public interface IExcelReportGenerator
{
    byte[] Generate(ExcelTable table);
}
