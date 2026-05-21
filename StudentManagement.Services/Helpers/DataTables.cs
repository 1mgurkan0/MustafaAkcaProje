namespace StudentManagement.Services.Helpers;
public class DataTableRequest { public int Draw { get; set; } public int Start { get; set; } public int Length { get; set; } public string Search { get; set; } = string.Empty; }
public class DataTableResponse<T> { public int Draw { get; set; } public int RecordsTotal { get; set; } public int RecordsFiltered { get; set; } public List<T> Data { get; set; } = new(); }
public class TalepRedViewModel { public int OgrenciDersId { get; set; } public string RedNedeni { get; set; } = string.Empty; }