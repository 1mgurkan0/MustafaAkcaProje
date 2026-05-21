namespace StudentManagement.Services.ViewModels.Ogrenci;

public class OgrenciListViewModel
{
    public int    Id          { get; set; }
    public string OgrenciNo  { get; set; } = null!;
    public string AdSoyad    { get; set; } = null!;
    public string Email      { get; set; } = null!;
    public string Bolum      { get; set; } = null!;
    public string Sinif      { get; set; } = null!;
    public string? Telefon   { get; set; }
    public DateTime KayitTarihi { get; set; }
    public int    DersSayisi { get; set; }
    public bool   IsActive   { get; set; }
}
