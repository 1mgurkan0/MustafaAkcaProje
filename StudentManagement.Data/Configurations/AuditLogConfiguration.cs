using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");
        builder.HasKey(a => a.Id);

        builder.HasIndex(a => a.KullaniciId);
        builder.HasIndex(a => a.Action);
        builder.HasIndex(a => a.Timestamp);

        builder.Property(a => a.KullaniciAdi).HasMaxLength(100);
        builder.Property(a => a.EntityName).HasMaxLength(100);
        builder.Property(a => a.IpAddress).HasMaxLength(50);
        builder.Property(a => a.Details).HasMaxLength(500);
        builder.Property(a => a.Action).HasConversion<int>();
    }
}
