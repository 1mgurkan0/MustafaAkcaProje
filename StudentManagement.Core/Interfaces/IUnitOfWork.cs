using StudentManagement.Core.Interfaces.Repositories;

namespace StudentManagement.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // ── Mevcut ───────────────────────────────────────────────────────────────
    IKullaniciRepository    Kullanicilar    { get; }
    IOgrenciRepository      Ogrenciler      { get; }
    IDersRepository         Dersler         { get; }
    IOgrenciDersRepository  OgrenciDersler  { get; }

    // ── UBYS — Yeni ──────────────────────────────────────────────────────────
    IBolumRepository        Bolumler        { get; }
    IDonemRepository        Donemler        { get; }
    IDersAtamaRepository    DersAtamalar    { get; }
    ISinavRepository        Sinavlar        { get; }
    IDuyuruRepository       Duyurular       { get; }
    IBelgeTalebiRepository  BelgeTalepleri  { get; }
    IYoklamaRepository      Yoklamalar      { get; }

    // ── Transaction ──────────────────────────────────────────────────────────
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
