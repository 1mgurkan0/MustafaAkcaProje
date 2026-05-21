using Microsoft.Extensions.Logging;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;
using StudentManagement.Data.Context;
using StudentManagement.Services.Interfaces;

namespace StudentManagement.Services.Helpers;

/// <summary>
/// AuditLog kayıtlarını direkt ApplicationDbContext üzerinden yazar.
/// UnitOfWork üzerinden değil — tasarım kararı.
/// </summary>
public class AuditLogWriter : IAuditLogWriter
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<AuditLogWriter> _logger;

    public AuditLogWriter(ApplicationDbContext db, ILogger<AuditLogWriter> logger)
    {
        _db     = db;
        _logger = logger;
    }

    public async Task WriteAsync(
        int userId,
        AuditAction action,
        string entity,
        int? entityId   = null,
        string? eskiDeger = null,
        string? yeniDeger = null,
        string? aciklama  = null)
    {
        try
        {
            var log = new AuditLog
            {
                KullaniciId = userId,
                Action      = action,
                EntityName  = entity,
                EntityId    = entityId,
                OldValues   = eskiDeger,
                NewValues   = yeniDeger,
                Details     = aciklama,
                Timestamp   = DateTime.UtcNow
            };

            _db.AuditLogs.Add(log);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // AuditLog hatası ana işlemi durdurmamalı
            _logger.LogError(ex, "AuditLog yazma hatası: Action={Action} Entity={Entity}", action, entity);
        }
    }
}
