namespace StudentManagement.Services.ViewModels.Common;

public class DataTableRequest
{
    public int    Draw          { get; set; }
    public int    Start         { get; set; }
    public int    Length        { get; set; }
    public string? Search       { get; set; }
    public string? SortColumn   { get; set; }
    public string? SortDirection { get; set; }
}

public class DataTableResponse<T>
{
    public int            Draw            { get; set; }
    public int            RecordsTotal    { get; set; }
    public int            RecordsFiltered { get; set; }
    public IEnumerable<T> Data            { get; set; } = Enumerable.Empty<T>();
}
