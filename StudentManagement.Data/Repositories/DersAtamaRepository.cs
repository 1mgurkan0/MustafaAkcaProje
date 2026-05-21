using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Repositories;

public class DersAtamaRepository : GenericRepository<DersAtama>, IDersAtamaRepository
{
    public DersAtamaRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<DersAtama>> GetByOgretmenAndDonemAsync(int ogretmenId, int donemId)
        => await _context.DersAtamalar
            .Include(da => da.Ders).ThenInclude(d => d.Bolum)
            .Include(da => da.Donem)
            .Where(da => da.OgretmenId == ogretmenId && da.DonemId == donemId)
            .OrderBy(da => da.Ders.DersKodu)
            .ToListAsync();

    public async Task<IEnumerable<DersAtama>> GetByDonemAsync(int donemId)
        => await _context.DersAtamalar
            .Include(da => da.Ders).ThenInclude(d => d.Bolum)
            .Include(da => da.Ogretmen)
            .Where(da => da.DonemId == donemId)
            .OrderBy(da => da.Ders.DersKodu)
            .ToListAsync();

    public async Task<IEnumerable<DersAtama>> GetByBolumAndDonemAsync(int bolumId, int donemId)
        => await _context.DersAtamalar
            .Include(da => da.Ders)
            .Include(da => da.Ogretmen)
            .Include(da => da.Donem)
            .Where(da => da.Ders.BolumId == bolumId && da.DonemId == donemId)
            .OrderBy(da => da.Ders.DersKodu)
            .ToListAsync();

    public async Task<DersAtama?> GetWithDetailsAsync(int id)
        => await _context.DersAtamalar
            .Include(da => da.Ders).ThenInclude(d => d.Bolum)
            .Include(da => da.Donem)
            .Include(da => da.Ogretmen)
            .Include(da => da.OgrenciDersler)
                .ThenInclude(od => od.Ogrenci).ThenInclude(o => o.Kullanici)
            .FirstOrDefaultAsync(da => da.Id == id);

    public async Task<bool> IsAlreadyAtanmisAsync(int dersId, int donemId, int? excludeId = null)
        => await _context.DersAtamalar
            .AnyAsync(da => da.DersId == dersId
                         && da.DonemId == donemId
                         && (excludeId == null || da.Id != excludeId));

    public async Task<bool> IsKontenjanDoluAsync(int dersAtamaId)
    {
        var atama = await _context.DersAtamalar
            .Include(da => da.Ders)
            .FirstOrDefaultAsync(da => da.Id == dersAtamaId);

        if (atama == null) return true;
        return atama.KayitliOgrenciSayisi >= atama.Ders.MaxKontenjan;
    }
}
