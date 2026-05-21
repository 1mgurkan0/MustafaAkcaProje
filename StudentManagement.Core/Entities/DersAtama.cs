using System.ComponentModel.DataAnnotations;
using StudentManagement.Core.Entities.Base;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Entities;

/// <summary>
/// Hangi öğretmenin, hangi dönemde, hangi dersi verdiğini tanımlar.
/// Ek olarak: gün/saat/derslik bilgisi de burada tutulur (ders programı).
/// Unique index: (DersId, DonemId) — bir ders bir dönemde tek öğretmene atanır.
/// </summary>
public class DersAtama : BaseEntity
{
    public int DersId { get; set; }

    public int DonemId { get; set; }

    /// <summary>Dersi veren öğretmenin KullaniciId'si</summary>
    public int OgretmenId { get; set; }

    // ── Program bilgisi ──────────────────────────────────────────────────────
    public DersGun? Gun { get; set; }

    public TimeSpan? BaslangicSaati { get; set; }

    public TimeSpan? BitisSaati { get; set; }

    [MaxLength(50)]
    public string? Derslik { get; set; }                  // A-101, Amfi-2, Lab-3

    /// <summary>
    /// Bu açılışa kayıtlı öğrenci sayısı.
    /// Ders.MaxKontenjan ile karşılaştırılır; dolduğunda yeni kayıt alınmaz.
    /// Onay bekleyenler sayılmaz — sadece Durum=Devam olanlar.
    /// </summary>
    public int KayitliOgrenciSayisi { get; set; } = 0;

    // ── Navigation ────────────────────────────────────────────────────────────
    public Ders Ders { get; set; } = null!;
    public Donem Donem { get; set; } = null!;
    public Kullanici Ogretmen { get; set; } = null!;

    public ICollection<OgrenciDers> OgrenciDersler { get; set; } = new HashSet<OgrenciDers>();
    public ICollection<Sinav> Sinavlar { get; set; } = new HashSet<Sinav>();
    public ICollection<Duyuru> Duyurular { get; set; } = new HashSet<Duyuru>();
    public ICollection<Yoklama> Yoklamalar { get; set; } = new HashSet<Yoklama>();
}
