using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class SinavConfiguration : IEntityTypeConfiguration<Sinav>
{
    public void Configure(EntityTypeBuilder<Sinav> builder)
    {
        builder.ToTable("Sinavlar");

        builder.HasIndex(s => new { s.DersAtamaId, s.SinavTur });
        builder.HasIndex(s => s.SinavTarihi);
        builder.HasIndex(s => s.IsActive);

        builder.Property(s => s.SinavTur).HasConversion<int>();
        builder.Property(s => s.Derslik).HasMaxLength(100);
        builder.Property(s => s.Aciklama).HasMaxLength(500);

        builder.HasOne(s => s.DersAtama)
               .WithMany(da => da.Sinavlar)
               .HasForeignKey(s => s.DersAtamaId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
