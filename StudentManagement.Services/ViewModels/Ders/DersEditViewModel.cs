using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Services.ViewModels.Ders;

public class DersEditViewModel
{
    public int Id { get; set; }

    [Required, MaxLength(20), Display(Name = "Ders Kodu")]
    public string DersKodu { get; set; } = null!;

    [Required, MaxLength(200), Display(Name = "Ders Adı")]
    public string DersAdi { get; set; } = null!;

    [Required, Range(1, 10), Display(Name = "Kredi")]
    public int Kredi { get; set; }

    [Required, Range(1, 30), Display(Name = "AKTS")]
    public int Akts { get; set; }

    [MaxLength(150), Display(Name = "Öğretmen Adı")]
    public string? OgretmenAdi { get; set; }

    [MaxLength(1000), Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }

    [MaxLength(50), Display(Name = "Dönem")]
    public string? Donem { get; set; }

    [Required, Range(1, 40), Display(Name = "Saatlik Ders Sayısı")]
    public int SaatlikDersSayisi { get; set; }
}
