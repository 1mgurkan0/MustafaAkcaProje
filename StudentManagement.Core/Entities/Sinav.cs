using System.ComponentModel.DataAnnotations;
using StudentManagement.Core.Entities.Base;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Entities;

public class Sinav : BaseEntity
{
    public int DersAtamaId { get; set; }

    public SinavTur SinavTur { get; set; }

    public DateTime SinavTarihi { get; set; }

    public TimeSpan BaslangicSaati { get; set; }

    public TimeSpan BitisSaati { get; set; }

    [MaxLength(100)]
    public string? Derslik { get; set; }

    [MaxLength(500)]
    public string? Aciklama { get; set; }

    // ── Navigation ────────────────────────────────────────────────────────────
    public DersAtama DersAtama { get; set; } = null!;
}
