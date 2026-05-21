using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Repositories;

public class YoklamaRepository : GenericRepository<Yoklama>, IYoklamaRepository
{
    public YoklamaRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Yoklama>> GetByDersAtamaAsync(int dersAtamaId)
        => await _context.Yoklamalar
            .Include(y => y.OgrenciYoklamalar)
            .Where(y => y.DersAtamaId == dersAtamaId)
            .OrderByDescending(y => y.YoklamaTarihi)
            .ToListAsync();

    public async Task<bool> IsYoklamaAlindıMiAsync(int dersAtamaId, DateTime tarih)
        => await _context.Yoklamalar
            .AnyAsync(y => y.DersAtamaId == dersAtamaId
                        && y.YoklamaTarihi.Date == tarih.Date);

    public async Task<Yoklama?> GetWithOgrenciYoklamalarAsync(int yoklamaId)
        => await _context.Yoklamalar
            .Include(y => y.OgrenciYoklamalar)
                .ThenInclude(oy => oy.Ogrenci)
                .ThenInclude(o => o.Kullanici)
            .FirstOrDefaultAsync(y => y.Id == yoklamaId);

    public async Task<(int Toplam, int Geldi, int Gelmedi)> GetDevamOzetiAsync(
        int ogrenciId, int dersAtamaId)
    {
        var yoklamalar = await _context.Set<OgrenciYoklama>()
            .Where(oy => oy.OgrenciId == ogrenciId
                      && oy.Yoklama.DersAtamaId == dersAtamaId)
            .ToListAsync();

        int toplam  = yoklamalar.Count;
        int geldi   = yoklamalar.Count(oy => oy.Geldi);
        int gelmedi = toplam - geldi;

        return (toplam, geldi, gelmedi);
    }
}
