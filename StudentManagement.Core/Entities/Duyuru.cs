using System.ComponentModel.DataAnnotations;
using StudentManagement.Core.Entities.Base;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Entities;

public class Duyuru : BaseEntity
{
    [Required, MaxLength(300)]
    public string Baslik { get; set; } = null!;

    [Required]
    public string Icerik { get; set; } = null!;

    public DuyuruHedef Hedef { get; set; } = DuyuruHedef.Herkes;

    /// <summary>Hedef = BolumOzeli ise hangi bölüm</summary>
    public int? HedefBolumId { get; set; }

    /// <summary>Hedef = DersOzeli ise hangi ders ataması</summary>
    public int? HedefDersAtamaId { get; set; }

    /// <summary>Yayınlayan kullanıcı (Admin veya Öğretmen)</summary>
    public int YayinlayanId { get; set; }

    public DateTime? YayinTarihi { get; set; }

    public DateTime? BitisTarihi { get; set; }

    public bool Onemli { get; set; } = false;       // Kırmızı badge ile gösterilsin mi

    // ── Navigation ────────────────────────────────────────────────────────────
    public Kullanici Yayinlayan { get; set; } = null!;
    public Bolum? HedefBolum { get; set; }
    public DersAtama? HedefDersAtama { get; set; }
}
