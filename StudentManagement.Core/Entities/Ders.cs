using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentManagement.Core.Entities.Base;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Entities;

/// <summary>
/// Ders kataloğu — dönem/hoca bağımsız tanım.
/// "BIL-101 Programlamaya Giriş" her dönem açılabilir; 
/// dönemsel detaylar DersAtama entity'sinde tutulur.
/// </summary>
public class Ders : BaseEntity
{
    [Required, MaxLength(20)]
    public string DersKodu { get; set; } = null!;         // BIL-101

    [Required, MaxLength(200)]
    public string DersAdi { get; set; } = null!;

    public int Kredi { get; set; }                         // Yerel kredi

    public int Akts { get; set; }                          // AKTS kredisi

    public int TeoriSaat { get; set; }                     // Haftalık teori saati

    public int UygulamaSaat { get; set; }                  // Haftalık lab/uygulama saati

    /// <summary>Bir bölümde aynı anda kayıt olabilecek max öğrenci</summary>
    public int MaxKontenjan { get; set; } = 30;

    /// <summary>
    /// Dersin ait olduğu bölüm.
    /// Öğrenci kendi BolumId'siyle eşleşen dersleri görebilir.
    /// </summary>
    public int BolumId { get; set; }

    [MaxLength(1000)]
    public string? Aciklama { get; set; }

    // ── Navigation ────────────────────────────────────────────────────────────
    public Bolum Bolum { get; set; } = null!;

    /// <summary>
    /// Bu dersin dönemsel açılışları (hangi dönemde kim veriyor, hangi sınıfta)
    /// </summary>
    public ICollection<DersAtama> DersAtamalari { get; set; } = new HashSet<DersAtama>();
}
