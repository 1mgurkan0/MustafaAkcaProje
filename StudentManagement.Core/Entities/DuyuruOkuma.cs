using StudentManagement.Core.Entities.Base;

namespace StudentManagement.Core.Entities;

/// <summary>
/// Öğrencinin bir duyuruyu okuduğunu işaretlemek için kullanılır.
/// </summary>
public class DuyuruOkuma : BaseEntity
{
    public int OgrenciId { get; set; }
    public int DuyuruId  { get; set; }

    /// <summary>Duyurunun okunduğu tarih (UTC)</summary>
    public DateTime OkunmaTarihi { get; set; } = DateTime.UtcNow;

    // ── Navigation ────────────────────────────────────────────────────────────
    public Ogrenci Ogrenci { get; set; } = null!;
    public Duyuru  Duyuru  { get; set; } = null!;
}
