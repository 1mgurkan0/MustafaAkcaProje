using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentManagement.Core.Entities.Base;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Entities;

/// <summary>
/// Öğrencinin belirli bir dönemde belirli bir derse kaydını temsil eder.
/// Unique index: (OgrenciId, DersAtamaId) — aynı dönemde aynı ders açılışına 2 kez kayıt olamaz.
/// </summary>
public class OgrenciDers : BaseEntity
{
    public int OgrenciId { get; set; }

    /// <summary>
    /// Ders + Dönem + Öğretmen üçlüsünü kapsayan DersAtama FK'sı.
    /// Eski DersId yerine bu kullanılır; dönem bilgisi DersAtama üzerinden gelir.
    /// </summary>
    public int DersAtamaId { get; set; }

    public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

    public OgrenciDersDurum Durum { get; set; } = OgrenciDersDurum.Talep;

    // ── Devam ─────────────────────────────────────────────────────────────────
    public int DevamSayisi { get; set; } = 0;
    public int DevamsizlikSayisi { get; set; } = 0;

    // ── Notlar (100'lük) ──────────────────────────────────────────────────────
    [Column(TypeName = "decimal(5,2)")]
    public decimal? VizeNotu { get; set; }          // Vize %40

    [Column(TypeName = "decimal(5,2)")]
    public decimal? FinalNotu { get; set; }         // Final %60

    [Column(TypeName = "decimal(5,2)")]
    public decimal? ButunlemeNotu { get; set; }     // Bütünleme (Final yerine geçer)

    /// <summary>
    /// Ağırlıklı genel not (100'lük).
    /// Vize*0.4 + Final*0.6 veya Vize*0.4 + But*0.6
    /// OgrenciDersService tarafından hesaplanır ve buraya yazılır.
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    public decimal? GenelNot { get; set; }

    /// <summary>4'lük harf notu — GenelNot'tan otomatik hesaplanır</summary>
    public HarfNotu? HarfNotu { get; set; }

    /// <summary>4'lük katsayı (AA=4.0, BA=3.5 ... FF=0.0)</summary>
    [Column(TypeName = "decimal(3,2)")]
    public decimal? NotKatsayisi { get; set; }

    public bool? GectiMi { get; set; }

    // ── Onay bilgisi (OgrenciIsleri tarafından) ───────────────────────────────
    public int? OnaylayanKullaniciId { get; set; }
    public DateTime? OnayTarihi { get; set; }

    [MaxLength(500)]
    public string? RedNedeni { get; set; }          // Reddedildiyse sebep

    // ── Navigation ────────────────────────────────────────────────────────────
    public Ogrenci Ogrenci { get; set; } = null!;
    public DersAtama DersAtama { get; set; } = null!;
    public Kullanici? OnaylayanKullanici { get; set; }
}
