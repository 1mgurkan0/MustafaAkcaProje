using System.ComponentModel.DataAnnotations;
using StudentManagement.Core.Entities.Base;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Entities;

public class BelgeTalebi : BaseEntity
{
    public int OgrenciId { get; set; }

    public BelgeTur BelgeTur { get; set; }

    public BelgeDurum Durum { get; set; } = BelgeDurum.Beklemede;

    /// <summary>Kaç adet isteniyor</summary>
    public int Adet { get; set; } = 1;

    [MaxLength(500)]
    public string? Aciklama { get; set; }               // Öğrencinin notu

    [MaxLength(500)]
    public string? SonucNotu { get; set; }              // Öğrenci işlerinin notu

    /// <summary>İşlemi yapan öğrenci işleri personeli</summary>
    public int? IslemYapanId { get; set; }

    public DateTime? TeslimTarihi { get; set; }

    // ── Navigation ────────────────────────────────────────────────────────────
    public Ogrenci Ogrenci { get; set; } = null!;
    public Kullanici? IslemYapan { get; set; }
}
