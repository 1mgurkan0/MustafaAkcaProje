using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.Core.Enums;

namespace StudentManagement.Services.ViewModels.Admin;

// ═══════════════════════════════════════════════════════════════════════════
// ÖĞRENCİ YÖNETİMİ (Admin/Ogrenciler)
// ═══════════════════════════════════════════════════════════════════════════

public class AdminOgrenciListViewModel
{
    public int OgrenciId { get; set; }
    public int KullaniciId { get; set; }
    public string OgrenciNo { get; set; } = null!;
    public string AdSoyad { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Telefon { get; set; }
    public string BolumAdi { get; set; } = null!;
    public string BolumKodu { get; set; } = null!;
    public int SinifSeviyesi { get; set; }
    public OgrenciDurum Durum { get; set; }
    public decimal? Gano { get; set; }
    public int TamamlananAkts { get; set; }
    public DateTime KayitTarihi { get; set; }
    public bool IsActive { get; set; }
    public int KayitliDersSayisi { get; set; }
}

public class AdminOgrenciFilterViewModel
{
    public string? Arama { get; set; }
    public int? BolumId { get; set; }
    public int? SinifSeviyesi { get; set; }
    public OgrenciDurum? Durum { get; set; }
    public List<AdminOgrenciListViewModel> Ogrenciler { get; set; } = new();
    public List<SelectListItem> BolumListesi { get; set; } = new();
}

public class AdminOgrenciDurumViewModel
{
    public int OgrenciId { get; set; }
    public string AdSoyad { get; set; } = null!;
    public OgrenciDurum YeniDurum { get; set; }
}

// ═══════════════════════════════════════════════════════════════════════════
// ÖĞRENCİ OLUŞTURMA (Admin/OgrenciOlustur)
// ═══════════════════════════════════════════════════════════════════════════

public class AdminOgrenciOlusturViewModel
{
    [Required(ErrorMessage = "Ad zorunludur.")]
    [MaxLength(100)]
    [Display(Name = "Ad")]
    public string Ad { get; set; } = null!;

    [Required(ErrorMessage = "Soyad zorunludur.")]
    [MaxLength(100)]
    [Display(Name = "Soyad")]
    public string Soyad { get; set; } = null!;

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [MaxLength(50)]
    [Display(Name = "Kullanıcı Adı")]
    public string KullaniciAdi { get; set; } = null!;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [MaxLength(256)]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    [Display(Name = "Şifre")]
    public string Sifre { get; set; } = null!;

    [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
    [Compare("Sifre", ErrorMessage = "Şifreler eşleşmiyor.")]
    [Display(Name = "Şifre Tekrar")]
    public string SifreTekrar { get; set; } = null!;

    [Required(ErrorMessage = "Bölüm seçiniz.")]
    [Display(Name = "Bölüm")]
    public int BolumId { get; set; }

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    [MaxLength(20)]
    [Display(Name = "Telefon")]
    public string? Telefon { get; set; }

    [Display(Name = "Doğum Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? DogumTarihi { get; set; }

    [MaxLength(10)]
    [Display(Name = "Cinsiyet")]
    public string? Cinsiyet { get; set; }

    [MaxLength(11)]
    [Display(Name = "TC Kimlik No")]
    public string? TcKimlikNo { get; set; }

    [Display(Name = "Sınıf Seviyesi")]
    [Range(1, 6, ErrorMessage = "Sınıf seviyesi 1-6 arasında olmalıdır.")]
    public int SinifSeviyesi { get; set; } = 1;

    // Dropdown için
    public List<SelectListItem> BolumListesi { get; set; } = new();
}

// ═══════════════════════════════════════════════════════════════════════════
// ÖĞRENCİ GÜNCELLEME (Admin/OgrenciGuncelle)
// ═══════════════════════════════════════════════════════════════════════════

public class AdminOgrenciGuncelleViewModel
{
    public int OgrenciId { get; set; }
    public int KullaniciId { get; set; }

    [Required(ErrorMessage = "Ad zorunludur.")]
    [MaxLength(100)]
    [Display(Name = "Ad")]
    public string Ad { get; set; } = null!;

    [Required(ErrorMessage = "Soyad zorunludur.")]
    [MaxLength(100)]
    [Display(Name = "Soyad")]
    public string Soyad { get; set; } = null!;

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [MaxLength(50)]
    [Display(Name = "Kullanıcı Adı")]
    public string KullaniciAdi { get; set; } = null!;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [MaxLength(256)]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = null!;

    [Display(Name = "Şifre (Değiştirmek istemiyorsanız boş bırakın)")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    public string? Sifre { get; set; }

    [Compare("Sifre", ErrorMessage = "Şifreler eşleşmiyor.")]
    [Display(Name = "Şifre Tekrar")]
    public string? SifreTekrar { get; set; }

    [Required(ErrorMessage = "Bölüm seçiniz.")]
    [Display(Name = "Bölüm")]
    public int BolumId { get; set; }

    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    [MaxLength(20)]
    [Display(Name = "Telefon")]
    public string? Telefon { get; set; }

    [Display(Name = "Doğum Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? DogumTarihi { get; set; }

    [MaxLength(10)]
    [Display(Name = "Cinsiyet")]
    public string? Cinsiyet { get; set; }

    [MaxLength(11)]
    [Display(Name = "TC Kimlik No")]
    public string? TcKimlikNo { get; set; }

    [Display(Name = "Sınıf Seviyesi")]
    [Range(1, 6, ErrorMessage = "Sınıf seviyesi 1-6 arasında olmalıdır.")]
    public int SinifSeviyesi { get; set; } = 1;

    // Dropdown için
    public List<SelectListItem> BolumListesi { get; set; } = new();
}

// ═══════════════════════════════════════════════════════════════════════════
// DERS KATALOĞU (Admin/DersKatalogu)
// ═══════════════════════════════════════════════════════════════════════════

public class AdminDersListViewModel
{
    public int DersId { get; set; }
    public string DersKodu { get; set; } = null!;
    public string DersAdi { get; set; } = null!;
    public string BolumAdi { get; set; } = null!;
    public string BolumKodu { get; set; } = null!;
    public int Kredi { get; set; }
    public int Akts { get; set; }
    public int TeoriSaat { get; set; }
    public int UygulamaSaat { get; set; }
    public int MaxKontenjan { get; set; }
    public int AtamaSayisi { get; set; }   // Kaç dönemde açıldı
    public bool IsActive { get; set; }
}

public class AdminDersFilterViewModel
{
    public string? Arama { get; set; }
    public int? BolumId { get; set; }
    public List<AdminDersListViewModel> Dersler { get; set; } = new();
    public List<SelectListItem> BolumListesi { get; set; } = new();
}

public class AdminDersCreateViewModel
{
    [Required(ErrorMessage = "Ders kodu zorunludur.")]
    [MaxLength(20)]
    [Display(Name = "Ders Kodu")]
    public string DersKodu { get; set; } = null!;

    [Required(ErrorMessage = "Ders adı zorunludur.")]
    [MaxLength(200)]
    [Display(Name = "Ders Adı")]
    public string DersAdi { get; set; } = null!;

    [Required(ErrorMessage = "Bölüm seçiniz.")]
    [Display(Name = "Bölüm")]
    public int BolumId { get; set; }

    [Display(Name = "Kredi")]
    [Range(1, 10)]
    public int Kredi { get; set; } = 3;

    [Display(Name = "AKTS")]
    [Range(1, 30)]
    public int Akts { get; set; } = 5;

    [Display(Name = "Teori Saati (Haftalık)")]
    [Range(0, 10)]
    public int TeoriSaat { get; set; } = 3;

    [Display(Name = "Uygulama Saati (Haftalık)")]
    [Range(0, 10)]
    public int UygulamaSaat { get; set; } = 0;

    [Display(Name = "Max Kontenjan")]
    [Range(5, 500)]
    public int MaxKontenjan { get; set; } = 30;

    [MaxLength(1000)]
    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }

    public List<SelectListItem> BolumListesi { get; set; } = new();
}

public class AdminDersEditViewModel : AdminDersCreateViewModel
{
    public int Id { get; set; }
}