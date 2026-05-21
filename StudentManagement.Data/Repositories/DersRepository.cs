using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Repositories;

public class DersRepository : GenericRepository<Ders>, IDersRepository
{
    public DersRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Ders?> GetByDersKoduAsync(string dersKodu)
        => await _context.Dersler
            .Include(d => d.Bolum)
            .FirstOrDefaultAsync(d => d.DersKodu == dersKodu);

    public async Task<bool> IsDersKoduUniqueAsync(string dersKodu, int? excludeId = null)
        => !await _context.Dersler
            .AnyAsync(d => d.DersKodu == dersKodu && (excludeId == null || d.Id != excludeId));

    public async Task<IEnumerable<Ders>> GetByBolumAsync(int bolumId)
        => await _context.Dersler
            .Include(d => d.Bolum)
            .Where(d => d.BolumId == bolumId)
            .OrderBy(d => d.DersKodu)
            .ToListAsync();

    public async Task<Ders?> GetWithAtamalarAsync(int id)
        => await _context.Dersler
            .Include(d => d.Bolum)
            .Include(d => d.DersAtamalari)
                .ThenInclude(da => da.Donem)
            .Include(d => d.DersAtamalari)
                .ThenInclude(da => da.Ogretmen)
            .FirstOrDefaultAsync(d => d.Id == id);

    public async Task<IEnumerable<Ders>> GetAllWithBolumAsync()
        => await _context.Dersler
            .Include(d => d.Bolum)
            .OrderBy(d => d.Bolum.BolumKodu)
            .ThenBy(d => d.DersKodu)
            .ToListAsync();
}
