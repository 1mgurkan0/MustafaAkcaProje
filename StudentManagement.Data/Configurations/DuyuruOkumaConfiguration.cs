using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class DuyuruOkumaConfiguration : IEntityTypeConfiguration<DuyuruOkuma>
{
    public void Configure(EntityTypeBuilder<DuyuruOkuma> builder)
    {
        builder.ToTable("DuyuruOkumalar");

        // Aynı öğrenci aynı duyuruyu birden fazla kez "okundu" işaretleyemesin
        builder.HasIndex(d => new { d.OgrenciId, d.DuyuruId }).IsUnique();
        builder.HasIndex(d => d.OgrenciId);
        builder.HasIndex(d => d.DuyuruId);

        builder.HasOne(d => d.Ogrenci)
               .WithMany()
               .HasForeignKey(d => d.OgrenciId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Duyuru)
               .WithMany()
               .HasForeignKey(d => d.DuyuruId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
