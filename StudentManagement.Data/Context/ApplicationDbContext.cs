using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Data.Configurations;

namespace StudentManagement.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // ── Mevcut ───────────────────────────────────────────────────────────────
    public DbSet<Kullanici>      Kullanicilar      => Set<Kullanici>();
    public DbSet<Ogrenci>        Ogrenciler        => Set<Ogrenci>();
    public DbSet<Ders>           Dersler           => Set<Ders>();
    public DbSet<OgrenciDers>    OgrenciDersler    => Set<OgrenciDers>();
    public DbSet<AuditLog>       AuditLogs         => Set<AuditLog>();

    // ── UBYS — Yeni ──────────────────────────────────────────────────────────
    public DbSet<Bolum>          Bolumler          => Set<Bolum>();
    public DbSet<Donem>          Donemler          => Set<Donem>();
    public DbSet<DersAtama>      DersAtamalar      => Set<DersAtama>();
    public DbSet<Yoklama>        Yoklamalar        => Set<Yoklama>();
    public DbSet<OgrenciYoklama> OgrenciYoklamalar => Set<OgrenciYoklama>();
    public DbSet<Sinav>          Sinavlar          => Set<Sinav>();
    public DbSet<Duyuru>         Duyurular         => Set<Duyuru>();
    public DbSet<BelgeTalebi>    BelgeTalepleri    => Set<BelgeTalebi>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Configuration'ları uygula ─────────────────────────────────────────
        modelBuilder.ApplyConfiguration(new KullaniciConfiguration());
        modelBuilder.ApplyConfiguration(new OgrenciConfiguration());
        modelBuilder.ApplyConfiguration(new DersConfiguration());
        modelBuilder.ApplyConfiguration(new OgrenciDersConfiguration());
        modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
        modelBuilder.ApplyConfiguration(new BolumConfiguration());
        modelBuilder.ApplyConfiguration(new DonemConfiguration());
        modelBuilder.ApplyConfiguration(new DersAtamaConfiguration());
        modelBuilder.ApplyConfiguration(new YoklamaConfiguration());
        modelBuilder.ApplyConfiguration(new OgrenciYoklamaConfiguration());
        modelBuilder.ApplyConfiguration(new SinavConfiguration());
        modelBuilder.ApplyConfiguration(new DuyuruConfiguration());
        modelBuilder.ApplyConfiguration(new BelgeTalebiConfiguration());

        // ── Global Query Filters (soft-delete) ───────────────────────────────
        modelBuilder.Entity<Kullanici>()   .HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Ogrenci>()     .HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Ders>()        .HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<OgrenciDers>() .HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Bolum>()       .HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Donem>()       .HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<DersAtama>()   .HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Yoklama>()     .HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Sinav>()       .HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Duyuru>()      .HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<BelgeTalebi>() .HasQueryFilter(e => e.IsActive);
        // AuditLog → filter YOK (intentional)
        // OgrenciYoklama → BaseEntity değil, filter YOK
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Core.Entities.Base.BaseEntity &&
                        e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            var entity = (Core.Entities.Base.BaseEntity)entry.Entity;
            entity.UpdatedAt = DateTime.UtcNow;
            if (entry.State == EntityState.Added)
                entity.CreatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
