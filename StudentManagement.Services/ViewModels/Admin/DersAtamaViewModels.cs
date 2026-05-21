using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.Core.Enums;

namespace StudentManagement.Services.ViewModels.Admin;

public class DersAtamaListViewModel
{
    public int      Id                  { get; set; }
    public string   DersKodu            { get; set; } = null!;
    public string   DersAdi             { get; set; } = null!;
    public string   BolumAdi            { get; set; } = null!;
    public string   OgretmenAdi         { get; set; } = null!;
    public string   DonemAdi            { get; set; } = null!;
    public string?  GunSaat             { get; set; }   // "Pazartesi 09:00-11:00"
    public string?  Derslik             { get; set; }
    public int      KayitliOgrenciSayisi { get; set; }
    public int      MaxKontenjan        { get; set; }
    public bool     IsActive            { get; set; }
}

public class DersAtamaCreateViewModel
{
    [Required(ErrorMessage = "Ders seçiniz.")]
    [Display(Name = "Ders")]
    public int DersId { get; set; }

    [Required(ErrorMessage = "Dönem seçiniz.")]
    [Display(Name = "Dönem")]
    public int DonemId { get; set; }

    [Required(ErrorMessage = "Öğretmen seçiniz.")]
    [Display(Name = "Öğretmen")]
    public int OgretmenId { get; set; }

    [Display(Name = "Gün")]
    public DersGun? Gun { get; set; }

    [Display(Name = "Başlangıç Saati")]
    [DataType(DataType.Time)]
    public TimeSpan? BaslangicSaati { get; set; }

    [Display(Name = "Bitiş Saati")]
    [DataType(DataType.Time)]
    public TimeSpan? BitisSaati { get; set; }

    [MaxLength(50)]
    [Display(Name = "Derslik")]
    public string? Derslik { get; set; }

    // Dropdown listeleri
    public List<SelectListItem> DersListesi      { get; set; } = new();
    public List<SelectListItem> DonemListesi     { get; set; } = new();
    public List<SelectListItem> OgretmenListesi  { get; set; } = new();
}

public class DersAtamaEditViewModel : DersAtamaCreateViewModel
{
    public int Id { get; set; }
}

