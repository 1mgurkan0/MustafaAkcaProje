using StudentManagement.Core.Entities.Base;

namespace StudentManagement.Core.Entities;

/// <summary>
/// Belirli bir DersAtama'nın belirli bir günündeki yoklama oturumu.
/// Her oturum için OgrenciYoklama kayıtları oluşturulur.
/// </summary>
public class Yoklama : BaseEntity
{
    public int DersAtamaId { get; set; }

    public DateTime YoklamaTarihi { get; set; }

    /// <summary>Yoklamayı alan öğretmen</summary>
    public int OgretmenId { get; set; }

    // ── Navigation ────────────────────────────────────────────────────────────
    public DersAtama DersAtama { get; set; } = null!;
    public Kullanici Ogretmen { get; set; } = null!;

    public ICollection<OgrenciYoklama> OgrenciYoklamalar { get; set; } = new HashSet<OgrenciYoklama>();
}
