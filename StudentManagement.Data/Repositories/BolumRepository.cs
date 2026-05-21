using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Repositories;

public class BolumRepository : GenericRepository<Bolum>, IBolumRepository
{
    public BolumRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Bolum?> GetByKodAsync(string bolumKodu)
        => await _context.Bolumler
            .FirstOrDefaultAsync(b => b.BolumKodu == bolumKodu);

    public async Task<bool> IsBolumKoduUniqueAsync(string bolumKodu, int? excludeId = null)
        => !await _context.Bolumler
            .AnyAsync(b => b.BolumKodu == bolumKodu && (excludeId == null || b.Id != excludeId));

    public async Task<IEnumerable<Bolum>> GetAllWithDerslerAsync()
        => await _context.Bolumler
            .Include(b => b.Dersler)
            .OrderBy(b => b.BolumAdi)
            .ToListAsync();

    public async Task<Bolum?> GetWithOgrencilerAsync(int id)
        => await _context.Bolumler
            .Include(b => b.Ogrenciler)
                .ThenInclude(o => o.Kullanici)
            .FirstOrDefaultAsync(b => b.Id == id);
}
