using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class DuyuruConfiguration : IEntityTypeConfiguration<Duyuru>
{
    public void Configure(EntityTypeBuilder<Duyuru> builder)
    {
        builder.ToTable("Duyurular");

        builder.HasIndex(d => d.Hedef);
        builder.HasIndex(d => d.HedefBolumId);
        builder.HasIndex(d => d.YayinlayanId);
        builder.HasIndex(d => d.IsActive);

        builder.Property(d => d.Baslik).IsRequired().HasMaxLength(300);
        builder.Property(d => d.Icerik).IsRequired();
        builder.Property(d => d.Hedef).HasConversion<int>();

        builder.HasOne(d => d.HedefBolum)
               .WithMany()
               .HasForeignKey(d => d.HedefBolumId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(d => d.HedefDersAtama)
               .WithMany(da => da.Duyurular)
               .HasForeignKey(d => d.HedefDersAtamaId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
