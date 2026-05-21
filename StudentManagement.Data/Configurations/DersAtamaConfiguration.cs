using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentManagement.Core.Entities;

namespace StudentManagement.Data.Configurations;

public class DersAtamaConfiguration : IEntityTypeConfiguration<DersAtama>
{
    public void Configure(EntityTypeBuilder<DersAtama> builder)
    {
        builder.ToTable("DersAtamalar");

        // Bir ders bir dönemde yalnızca bir kez açılabilir
        builder.HasIndex(da => new { da.DersId, da.DonemId }).IsUnique();
        builder.HasIndex(da => da.OgretmenId);
        builder.HasIndex(da => da.IsActive);

        builder.Property(da => da.Derslik).HasMaxLength(50);
        builder.Property(da => da.Gun).HasConversion<int>();

        // ── İlişkiler ─────────────────────────────────────────────────────────
        builder.HasOne(da => da.Ders)
               .WithMany(d => d.DersAtamalari)
               .HasForeignKey(da => da.DersId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(da => da.Donem)
               .WithMany(d => d.DersAtamalari)
               .HasForeignKey(da => da.DonemId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(da => da.Ogretmen)
               .WithMany(k => k.DersAtamalari)
               .HasForeignKey(da => da.OgretmenId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
