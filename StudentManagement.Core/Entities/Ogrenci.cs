using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentManagement.Core.Entities.Base;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Entities;

public class Ogrenci : BaseEntity
{
    public int KullaniciId { get; set; }

    [Required, MaxLength(20)]
    public string OgrenciNo { get; set; } = null!;   // 2024001 — unique, otomatik üretilir

    /// <summary>
    /// Öğrencinin kayıtlı olduğu bölüm (FK → Bolum).
    /// Eski sistemdeki string Bolum alanının yerini alır.
    /// </summary>
    public int BolumId { get; set; }

    /// <summary>
    /// Öğrencinin aktif kaydının bulunduğu dönem (FK → Donem).
    /// </summary>
    public int? AktifDonemId { get; set; }

    /// <summary>1, 2, 3, 4 — sınıf seviyesi</summary>
    public int SinifSeviyesi { get; set; } = 1;

    public OgrenciDurum Durum { get; set; } = OgrenciDurum.Aktif;

    [MaxLength(11)]
    public string? TcKimlikNo { get; set; }

    [MaxLength(500)]
    public string? ProfilFotoUrl { get; set; }

    public DateTime DogumTarihi { get; set; }

    [MaxLength(10)]
    public string? Cinsiyet { get; set; }

    public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Genel Ağırlıklı Not Ortalaması (4'lük sistem).
    /// Her not girişinde OgrenciDersService tarafından yeniden hesaplanır.
    /// </summary>
    [Column(TypeName = "decimal(4,2)")]
    public decimal? Gano { get; set; }

    /// <summary>Tamamlanan toplam AKTS kredisi</summary>
    public int TamamlananAkts { get; set; } = 0;

    // ── Navigation ────────────────────────────────────────────────────────────
    public Kullanici Kullanici { get; set; } = null!;
    public Bolum Bolum { get; set; } = null!;
    public Donem? AktifDonem { get; set; }

    public ICollection<OgrenciDers> OgrenciDersler { get; set; } = new HashSet<OgrenciDers>();
    public ICollection<BelgeTalebi> BelgeTalepleri { get; set; } = new HashSet<BelgeTalebi>();
}
