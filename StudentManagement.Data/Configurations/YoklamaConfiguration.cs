using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class YoklamaConfiguration : IEntityTypeConfiguration<Yoklama>
{
    public void Configure(EntityTypeBuilder<Yoklama> builder)
    {
        builder.ToTable("Yoklamalar");

        builder.HasIndex(y => new { y.DersAtamaId, y.YoklamaTarihi });
        builder.HasIndex(y => y.IsActive);

        builder.HasOne(y => y.DersAtama)
               .WithMany(da => da.Yoklamalar)
               .HasForeignKey(y => y.DersAtamaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(y => y.Ogretmen)
               .WithMany()
               .HasForeignKey(y => y.OgretmenId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
