using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class DersConfiguration : IEntityTypeConfiguration<Ders>
{
    public void Configure(EntityTypeBuilder<Ders> builder)
    {
        builder.ToTable("Dersler");

        builder.HasIndex(d => d.DersKodu).IsUnique();
        builder.HasIndex(d => d.BolumId);
        builder.HasIndex(d => d.IsActive);

        builder.Property(d => d.DersKodu).IsRequired().HasMaxLength(20);
        builder.Property(d => d.DersAdi).IsRequired().HasMaxLength(200);
        builder.Property(d => d.Aciklama).HasMaxLength(1000);

        // ── İlişkiler ─────────────────────────────────────────────────────────
        builder.HasOne(d => d.Bolum)
               .WithMany(b => b.Dersler)
               .HasForeignKey(d => d.BolumId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
