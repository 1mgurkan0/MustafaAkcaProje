using System.ComponentModel.DataAnnotations;
using StudentManagement.Core.Entities.Base;

namespace StudentManagement.Core.Entities;

public class Bolum : BaseEntity
{
    [Required, MaxLength(20)]
    public string BolumKodu { get; set; } = null!;       // BIL, EEE, MAT...

    [Required, MaxLength(200)]
    public string BolumAdi { get; set; } = null!;         // Bilgisayar Mühendisliği

    [MaxLength(200)]
    public string? Fakulte { get; set; }                  // Mühendislik Fakültesi

    [MaxLength(500)]
    public string? Aciklama { get; set; }

    /// <summary>Bölümün toplam mezuniyet için gereken minimum AKTS kredisi</summary>
    public int MinMezuniyetAkts { get; set; } = 240;

    // ── Navigation ────────────────────────────────────────────────────────────
    public ICollection<Ders> Dersler { get; set; } = new HashSet<Ders>();
    public ICollection<Ogrenci> Ogrenciler { get; set; } = new HashSet<Ogrenci>();

    /// <summary>Bu bölüme atanmış öğretmenler</summary>
    public ICollection<Kullanici> Ogretmenler { get; set; } = new HashSet<Kullanici>();
}
