using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class OgrenciConfiguration : IEntityTypeConfiguration<Ogrenci>
{
    public void Configure(EntityTypeBuilder<Ogrenci> builder)
    {
        builder.ToTable("Ogrenciler");

        builder.HasIndex(o => o.OgrenciNo).IsUnique();
        builder.HasIndex(o => o.KullaniciId).IsUnique();
        builder.HasIndex(o => o.BolumId);
        builder.HasIndex(o => o.IsActive);

        builder.Property(o => o.OgrenciNo).IsRequired().HasMaxLength(20);
        builder.Property(o => o.Cinsiyet).HasMaxLength(10);
        builder.Property(o => o.Gano).HasColumnType("decimal(4,2)");
        builder.Property(o => o.Durum).HasConversion<int>();

        // ── İlişkiler ─────────────────────────────────────────────────────────
        builder.HasOne(o => o.Kullanici)
               .WithOne(k => k.Ogrenci)
               .HasForeignKey<Ogrenci>(o => o.KullaniciId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Bolum)
               .WithMany(b => b.Ogrenciler)
               .HasForeignKey(o => o.BolumId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.AktifDonem)
               .WithMany()
               .HasForeignKey(o => o.AktifDonemId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
