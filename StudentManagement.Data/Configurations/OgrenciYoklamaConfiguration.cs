using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class OgrenciYoklamaConfiguration : IEntityTypeConfiguration<OgrenciYoklama>
{
    public void Configure(EntityTypeBuilder<OgrenciYoklama> builder)
    {
        builder.ToTable("OgrenciYoklamalar");

        builder.HasKey(oy => oy.Id);
        builder.HasIndex(oy => new { oy.YoklamaId, oy.OgrenciId }).IsUnique();
        builder.HasIndex(oy => oy.OgrenciId);

        builder.Property(oy => oy.Aciklama).HasMaxLength(200);

        builder.HasOne(oy => oy.Yoklama)
               .WithMany(y => y.OgrenciYoklamalar)
               .HasForeignKey(oy => oy.YoklamaId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(oy => oy.Ogrenci)
               .WithMany()
               .HasForeignKey(oy => oy.OgrenciId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
