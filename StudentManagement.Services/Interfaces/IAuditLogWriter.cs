using StudentManagement.Core.Enums;

namespace StudentManagement.Services.Interfaces;

public interface IAuditLogWriter
{
    Task WriteAsync(
        int userId,
        AuditAction action,
        string entity,
        int? entityId = null,
        string? eskiDeger = null,
        string? yeniDeger = null,
        string? aciklama = null);
}