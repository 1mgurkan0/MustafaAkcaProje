using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Services.ViewModels.Ogrenci;

public class OgrenciEditViewModel
{
    public int Id { get; set; }
    public int KullaniciId { get; set; }

    [Required, MaxLength(20), Display(Name = "Öğrenci No")]
    public string OgrenciNo { get; set; } = null!;

    [Required, MaxLength(150), Display(Name = "Bölüm")]
    public string Bolum { get; set; } = null!;

    [Required, MaxLength(50), Display(Name = "Sınıf")]
    public string Sinif { get; set; } = null!;

    [Required, DataType(DataType.Date), Display(Name = "Doğum Tarihi")]
    public DateTime DogumTarihi { get; set; }

    [MaxLength(10), Display(Name = "Cinsiyet")]
    public string? Cinsiyet { get; set; }

    [Required, MaxLength(100), Display(Name = "Ad")]
    public string Ad { get; set; } = null!;

    [Required, MaxLength(100), Display(Name = "Soyad")]
    public string Soyad { get; set; } = null!;

    [Required, EmailAddress, MaxLength(256), Display(Name = "E-Posta")]
    public string Email { get; set; } = null!;

    [MaxLength(20), Phone, Display(Name = "Telefon")]
    public string? Telefon { get; set; }
}
