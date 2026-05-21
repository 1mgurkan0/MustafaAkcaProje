using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Repositories;

public class SinavRepository : GenericRepository<Sinav>, ISinavRepository
{
    public SinavRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Sinav>> GetByDersAtamaAsync(int dersAtamaId)
        => await _context.Sinavlar
            .Include(s => s.DersAtama).ThenInclude(da => da.Ders)
            .Where(s => s.DersAtamaId == dersAtamaId)
            .OrderBy(s => s.SinavTarihi)
            .ToListAsync();

    public async Task<IEnumerable<Sinav>> GetByOgrenciAsync(int ogrenciId, int donemId)
        => await _context.Sinavlar
            .Include(s => s.DersAtama).ThenInclude(da => da.Ders)
            .Where(s => s.DersAtama.DonemId == donemId
                     && s.DersAtama.OgrenciDersler
                           .Any(od => od.OgrenciId == ogrenciId
                                   && od.Durum == Core.Enums.OgrenciDersDurum.Devam))
            .OrderBy(s => s.SinavTarihi)
            .ToListAsync();

    public async Task<IEnumerable<Sinav>> GetByOgretmenAsync(int ogretmenId, int donemId)
        => await _context.Sinavlar
            .Include(s => s.DersAtama).ThenInclude(da => da.Ders)
            .Include(s => s.DersAtama).ThenInclude(da => da.Donem)
            .Where(s => s.DersAtama.OgretmenId == ogretmenId
                     && s.DersAtama.DonemId == donemId)
            .OrderBy(s => s.SinavTarihi)
            .ToListAsync();

    public async Task<IEnumerable<Sinav>> GetByTarihAraligiAsync(DateTime baslangic, DateTime bitis)
        => await _context.Sinavlar
            .Include(s => s.DersAtama).ThenInclude(da => da.Ders)
            .Include(s => s.DersAtama).ThenInclude(da => da.Ogretmen)
            .Where(s => s.SinavTarihi >= baslangic && s.SinavTarihi <= bitis)
            .OrderBy(s => s.SinavTarihi)
            .ToListAsync();
}
