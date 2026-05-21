using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class BelgeTalebiConfiguration : IEntityTypeConfiguration<BelgeTalebi>
{
    public void Configure(EntityTypeBuilder<BelgeTalebi> builder)
    {
        builder.ToTable("BelgeTalepleri");

        builder.HasIndex(b => b.OgrenciId);
        builder.HasIndex(b => b.Durum);
        builder.HasIndex(b => b.IsActive);

        builder.Property(b => b.BelgeTur).HasConversion<int>();
        builder.Property(b => b.Durum).HasConversion<int>();
        builder.Property(b => b.Aciklama).HasMaxLength(500);
        builder.Property(b => b.SonucNotu).HasMaxLength(500);

        builder.HasOne(b => b.Ogrenci)
               .WithMany(o => o.BelgeTalepleri)
               .HasForeignKey(b => b.OgrenciId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.IslemYapan)
               .WithMany()
               .HasForeignKey(b => b.IslemYapanId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
