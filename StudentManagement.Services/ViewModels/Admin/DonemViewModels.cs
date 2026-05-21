using System.ComponentModel.DataAnnotations;
using StudentManagement.Core.Enums;

namespace StudentManagement.Services.ViewModels.Admin;

public class DonemListViewModel
{
    public int      Id                { get; set; }
    public string   DonemKodu         { get; set; } = null!;
    public string   DonemAdi          { get; set; } = null!;
    public int      Yil               { get; set; }
    public DonemTur DonemTur          { get; set; }
    public bool     AktifMi           { get; set; }
    public DateTime BaslangicTarihi   { get; set; }
    public DateTime BitisTarihi       { get; set; }
    public DateTime DersKayitBaslangic { get; set; }
    public DateTime DersKayitBitis    { get; set; }
    public int      DersAtamaSayisi   { get; set; }
    public int      KayitliOgrenci    { get; set; }
}

public class DonemCreateViewModel
{
    [Required(ErrorMessage = "Dönem kodu zorunludur.")]
    [MaxLength(20)]
    [Display(Name = "Dönem Kodu")]
    public string DonemKodu { get; set; } = null!;

    [Required(ErrorMessage = "Dönem adı zorunludur.")]
    [MaxLength(100)]
    [Display(Name = "Dönem Adı")]
    public string DonemAdi { get; set; } = null!;

    [Display(Name = "Yıl")]
    [Range(2020, 2100)]
    public int Yil { get; set; } = DateTime.Now.Year;

    [Display(Name = "Dönem Türü")]
    public DonemTur DonemTur { get; set; }

    [Display(Name = "Başlangıç Tarihi")]
    [DataType(DataType.Date)]
    public DateTime BaslangicTarihi { get; set; }

    [Display(Name = "Bitiş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime BitisTarihi { get; set; }

    [Display(Name = "Kayıt Başlangıcı")]
    [DataType(DataType.Date)]
    public DateTime DersKayitBaslangic { get; set; }

    [Display(Name = "Kayıt Bitişi")]
    [DataType(DataType.Date)]
    public DateTime DersKayitBitis { get; set; }
}

public class DonemEditViewModel : DonemCreateViewModel
{
    public int  Id      { get; set; }
    public bool AktifMi { get; set; }
}
