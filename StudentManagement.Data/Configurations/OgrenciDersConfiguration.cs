using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class OgrenciDersConfiguration : IEntityTypeConfiguration<OgrenciDers>
{
    public void Configure(EntityTypeBuilder<OgrenciDers> builder)
    {
        builder.ToTable("OgrenciDersler");

        // GÜNCELLEME: DersId → DersAtamaId (dönem + hoca bilgisi buradan)
        builder.HasIndex(od => new { od.OgrenciId, od.DersAtamaId }).IsUnique();
        builder.HasIndex(od => od.Durum);
        builder.HasIndex(od => od.IsActive);
        builder.HasIndex(od => od.OnaylayanKullaniciId);

        // Not alanları
        builder.Property(od => od.VizeNotu).HasColumnType("decimal(5,2)");
        builder.Property(od => od.FinalNotu).HasColumnType("decimal(5,2)");
        builder.Property(od => od.ButunlemeNotu).HasColumnType("decimal(5,2)");
        builder.Property(od => od.GenelNot).HasColumnType("decimal(5,2)");
        builder.Property(od => od.NotKatsayisi).HasColumnType("decimal(3,2)");
        builder.Property(od => od.Durum).HasConversion<int>();
        builder.Property(od => od.HarfNotu).HasConversion<int>();
        builder.Property(od => od.RedNedeni).HasMaxLength(500);

        // ── İlişkiler ─────────────────────────────────────────────────────────
        builder.HasOne(od => od.Ogrenci)
               .WithMany(o => o.OgrenciDersler)
               .HasForeignKey(od => od.OgrenciId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(od => od.DersAtama)
               .WithMany(da => da.OgrenciDersler)
               .HasForeignKey(od => od.DersAtamaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(od => od.OnaylayanKullanici)
               .WithMany()
               .HasForeignKey(od => od.OnaylayanKullaniciId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
