using System.ComponentModel.DataAnnotations;
using StudentManagement.Core.Entities.Base;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Entities;

public class Donem : BaseEntity
{
    /// <summary>2024-2025 Güz → "20241" gibi kısa kod. Unique.</summary>
    [Required, MaxLength(20)]
    public string DonemKodu { get; set; } = null!;

    /// <summary>2024-2025 Güz Dönemi</summary>
    [Required, MaxLength(100)]
    public string DonemAdi { get; set; } = null!;

    public int Yil { get; set; }                          // 2024

    public DonemTur DonemTur { get; set; }                // Guz / Bahar / Yaz

    public DateTime BaslangicTarihi { get; set; }

    public DateTime BitisTarihi { get; set; }

    /// <summary>Ders kaydının açık olduğu tarih aralığı</summary>
    public DateTime DersKayitBaslangic { get; set; }
    public DateTime DersKayitBitis { get; set; }

    /// <summary>
    /// True ise şu an aktif dönem.
    /// Sistemde yalnızca 1 aktif dönem olabilir (DbContext veya service katmanında kontrol edilir).
    /// </summary>
    public bool AktifMi { get; set; } = false;

    // ── Navigation ────────────────────────────────────────────────────────────
    public ICollection<DersAtama> DersAtamalari { get; set; } = new HashSet<DersAtama>();
    public ICollection<OgrenciDers> OgrenciDersler { get; set; } = new HashSet<OgrenciDers>();
}
