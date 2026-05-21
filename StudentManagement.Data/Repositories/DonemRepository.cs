using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Repositories;

public class DonemRepository : GenericRepository<Donem>, IDonemRepository
{
    public DonemRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Donem?> GetAktifDonemAsync()
        => await _context.Donemler
            .FirstOrDefaultAsync(d => d.AktifMi);

    public async Task<Donem?> GetByKodAsync(string donemKodu)
        => await _context.Donemler
            .FirstOrDefaultAsync(d => d.DonemKodu == donemKodu);

    public async Task<IEnumerable<Donem>> GetAllOrderedAsync()
        => await _context.Donemler
            .OrderByDescending(d => d.Yil)
            .ThenBy(d => d.DonemTur)
            .ToListAsync();

    public async Task<bool> IsDonemKoduUniqueAsync(string donemKodu, int? excludeId = null)
        => !await _context.Donemler
            .AnyAsync(d => d.DonemKodu == donemKodu && (excludeId == null || d.Id != excludeId));

    public async Task ClearAktifDonemAsync()
    {
        var aktifler = await _context.Donemler
            .Where(d => d.AktifMi)
            .ToListAsync();

        foreach (var d in aktifler)
            d.AktifMi = false;
    }
}
