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