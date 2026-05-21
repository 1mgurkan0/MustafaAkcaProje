using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class DonemConfiguration : IEntityTypeConfiguration<Donem>
{
    public void Configure(EntityTypeBuilder<Donem> builder)
    {
        builder.ToTable("Donemler");

        builder.HasIndex(d => d.DonemKodu).IsUnique();
        builder.HasIndex(d => d.AktifMi);
        builder.HasIndex(d => d.IsActive);

        builder.Property(d => d.DonemKodu).IsRequired().HasMaxLength(20);
        builder.Property(d => d.DonemAdi).IsRequired().HasMaxLength(100);
        builder.Property(d => d.DonemTur).HasConversion<int>();
    }
}
