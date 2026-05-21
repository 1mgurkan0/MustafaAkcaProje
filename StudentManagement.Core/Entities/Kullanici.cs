using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentManagement.Core.Entities.Base;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Entities;

public class Kullanici : BaseEntity
{
    [Required, MaxLength(50)]
    public string KullaniciAdi { get; set; } = null!;

    [Required, MaxLength(256)]
    public string SifreHash { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Ad { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Soyad { get; set; } = null!;

    [Required, MaxLength(256)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [MaxLength(20)]
    [Phone]
    public string? Telefon { get; set; }

    public KullaniciRol Rol { get; set; } = KullaniciRol.Ogrenci;

    /// <summary>
    /// Öğretmen unvanı: Prof.Dr. / Doç.Dr. / Dr.Öğr.Üyesi / Arş.Gör. / Öğr.Gör.
    /// Sadece Ogretmen ve OgrenciIsleri rolleri için anlamlıdır.
    /// </summary>
    [MaxLength(50)]
    public string? Unvan { get; set; }

    /// <summary>
    /// Öğretmenin bağlı olduğu bölüm (FK).
    /// Öğrenci için Ogrenci.BolumId kullanılır; bu alan Ogretmen/OgrenciIsleri içindir.
    /// </summary>
    public int? BolumId { get; set; }

    public DateTime? SonGirisTarihi { get; set; }

    public DateTime? LastPasswordChangeDate { get; set; }

    public int FailedLoginAttempts { get; set; } = 0;

    public bool IsLocked { get; set; } = false;

    public DateTime? LockoutEndTime { get; set; }

    // ── Computed (migration dışı) ─────────────────────────────────────────────
    [NotMapped]
    public string TamAd => $"{Ad} {Soyad}";

    /// <summary>
    /// Unvanı varsa "Prof.Dr. Ahmet Yılmaz", yoksa sadece "Ahmet Yılmaz"
    /// </summary>
    [NotMapped]
    public string UnvanliAd => string.IsNullOrWhiteSpace(Unvan)
        ? TamAd
        : $"{Unvan} {TamAd}";

    // ── Navigation ────────────────────────────────────────────────────────────
    public Bolum? Bolum { get; set; }
    public Ogrenci? Ogrenci { get; set; }

    /// <summary>Öğretmenin dönemsel ders atamaları</summary>
    public ICollection<DersAtama> DersAtamalari { get; set; } = new HashSet<DersAtama>();

    public ICollection<AuditLog> AuditLogs { get; set; } = new HashSet<AuditLog>();
    public ICollection<Duyuru> Duyurular { get; set; } = new HashSet<Duyuru>();
}
