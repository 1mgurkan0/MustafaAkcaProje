using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Core.Entities;

/// <summary>
/// Bir yoklama oturumunda öğrencinin durumu.
/// BaseEntity'den türemez — soft-delete, CreatedBy gibi alanlara gerek yok.
/// Unique index: (YoklamaId, OgrenciId)
/// </summary>
public class OgrenciYoklama
{
    public int Id { get; set; }

    public int YoklamaId { get; set; }

    public int OgrenciId { get; set; }

    /// <summary>True = Geldi, False = Gelmedi</summary>
    public bool Geldi { get; set; } = false;

    [MaxLength(200)]
    public string? Aciklama { get; set; }   // Mazeretli, İzinli vb.

    public DateTime KayitTarihi { get; set; } = DateTime.UtcNow;

    // ── Navigation ────────────────────────────────────────────────────────────
    public Yoklama Yoklama { get; set; } = null!;
    public Ogrenci Ogrenci { get; set; } = null!;
}
