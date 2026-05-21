namespace StudentManagement.Services.ViewModels.Admin;

public class DersListViewModel
{
    public int Id { get; set; }
    public string DersKodu { get; set; } = string.Empty;
    public string DersAdi { get; set; } = string.Empty;
    public string BolumAdi { get; set; } = string.Empty;
    public int Akts { get; set; }
    public int TeorikSaat { get; set; }
    public int UygulamaSaat { get; set; }
    public int OgrenciSayisi { get; set; }
}