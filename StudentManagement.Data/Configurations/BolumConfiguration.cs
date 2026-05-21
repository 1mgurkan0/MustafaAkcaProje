using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class BolumConfiguration : IEntityTypeConfiguration<Bolum>
{
    public void Configure(EntityTypeBuilder<Bolum> builder)
    {
        builder.ToTable("Bolumler");

        builder.HasIndex(b => b.BolumKodu).IsUnique();
        builder.HasIndex(b => b.IsActive);

        builder.Property(b => b.BolumKodu).IsRequired().HasMaxLength(20);
        builder.Property(b => b.BolumAdi).IsRequired().HasMaxLength(200);
        builder.Property(b => b.Fakulte).HasMaxLength(200);
        builder.Property(b => b.Aciklama).HasMaxLength(500);
    }
}
