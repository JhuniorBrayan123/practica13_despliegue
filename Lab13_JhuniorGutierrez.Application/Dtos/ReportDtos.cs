namespace Lab13_JhuniorGutierrez.Application.Dtos;

public record ExcelFileDto(byte[] Content, string FileName)
{
    public const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
}

public record ExcelTable(
    string SheetName,
    IReadOnlyList<string> Headers,
    IReadOnlyList<IReadOnlyList<object?>> Rows);
