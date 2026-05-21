using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Services.ViewModels.Admin;

public class BolumListViewModel
{
    public int    Id             { get; set; }
    public string BolumKodu     { get; set; } = null!;
    public string BolumAdi      { get; set; } = null!;
    public string? Fakulte      { get; set; }
    public int    OgrenciSayisi  { get; set; }
    public int    DersSayisi     { get; set; }
    public int    OgretmenSayisi { get; set; }
    public bool   IsActive       { get; set; }
}

public class BolumCreateViewModel
{
    [Required(ErrorMessage = "Bölüm kodu zorunludur.")]
    [MaxLength(20)]
    [Display(Name = "Bölüm Kodu")]
    public string BolumKodu { get; set; } = null!;

    [Required(ErrorMessage = "Bölüm adı zorunludur.")]
    [MaxLength(200)]
    [Display(Name = "Bölüm Adı")]
    public string BolumAdi { get; set; } = null!;

    [MaxLength(200)]
    [Display(Name = "Fakülte")]
    public string? Fakulte { get; set; }

    [MaxLength(500)]
    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }

    [Display(Name = "Min. Mezuniyet AKTS")]
    [Range(60, 360)]
    public int MinMezuniyetAkts { get; set; } = 240;
}

public class BolumEditViewModel : BolumCreateViewModel
{
    public int Id { get; set; }
}
