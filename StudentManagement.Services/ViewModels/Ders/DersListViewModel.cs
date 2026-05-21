namespace StudentManagement.Services.ViewModels.Ders;

public class DersListViewModel
{
    public int    Id               { get; set; }
    public string DersKodu        { get; set; } = null!;
    public string DersAdi         { get; set; } = null!;
    public int    Kredi           { get; set; }
    public int    Akts            { get; set; }
    public string? OgretmenAdi    { get; set; }
    public string? Donem          { get; set; }
    public int    SaatlikDersSayisi { get; set; }
    public int    OgrenciSayisi   { get; set; }
    public bool   IsActive        { get; set; }
}
