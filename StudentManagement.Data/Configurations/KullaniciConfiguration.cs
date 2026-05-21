using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class KullaniciConfiguration : IEntityTypeConfiguration<Kullanici>
{
    public void Configure(EntityTypeBuilder<Kullanici> builder)
    {
        builder.ToTable("Kullanicilar");

        builder.HasIndex(k => k.KullaniciAdi).IsUnique();
        builder.HasIndex(k => k.Email).IsUnique();
        builder.HasIndex(k => k.Rol);
        builder.HasIndex(k => k.IsActive);

        builder.Property(k => k.KullaniciAdi).IsRequired().HasMaxLength(50);
        builder.Property(k => k.SifreHash).IsRequired().HasMaxLength(256);
        builder.Property(k => k.Ad).IsRequired().HasMaxLength(100);
        builder.Property(k => k.Soyad).IsRequired().HasMaxLength(100);
        builder.Property(k => k.Email).IsRequired().HasMaxLength(256);
        builder.Property(k => k.Telefon).HasMaxLength(20);
        builder.Property(k => k.Unvan).HasMaxLength(50);
        builder.Property(k => k.Rol).HasConversion<int>();

        // ── İlişkiler ─────────────────────────────────────────────────────────
        builder.HasOne(k => k.Bolum)
               .WithMany(b => b.Ogretmenler)
               .HasForeignKey(k => k.BolumId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(k => k.AuditLogs)
               .WithOne(a => a.Kullanici)
               .HasForeignKey(a => a.KullaniciId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(k => k.Duyurular)
               .WithOne(d => d.Yayinlayan)
               .HasForeignKey(d => d.YayinlayanId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
