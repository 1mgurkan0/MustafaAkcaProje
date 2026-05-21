using System.ComponentModel.DataAnnotations;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public int? KullaniciId { get; set; }

    [MaxLength(100)]
    public string? KullaniciAdi { get; set; }

    public AuditAction Action { get; set; }

    [MaxLength(100)]
    public string? EntityName { get; set; }

    public int? EntityId { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string? IpAddress { get; set; }

    public string? OldValues { get; set; }
    public string? NewValues { get; set; }

    [MaxLength(500)]
    public string? Details { get; set; }

    public Kullanici? Kullanici { get; set; }
}
