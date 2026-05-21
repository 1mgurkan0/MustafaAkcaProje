using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Repositories;

public class OgrenciDersRepository : GenericRepository<OgrenciDers>, IOgrenciDersRepository
{
    public OgrenciDersRepository(ApplicationDbContext context) : base(context) { }

    public async Task<OgrenciDers?> GetByOgrenciAndDersAtamaAsync(int ogrenciId, int dersAtamaId)
        => await _context.OgrenciDersler
            .Include(od => od.DersAtama).ThenInclude(da => da.Ders)
            .Include(od => od.DersAtama).ThenInclude(da => da.Donem)
            .FirstOrDefaultAsync(od => od.OgrenciId == ogrenciId && od.DersAtamaId == dersAtamaId);

    public async Task<IEnumerable<OgrenciDers>> GetByOgrenciIdAsync(int ogrenciId)
        => await _context.OgrenciDersler
            .Include(od => od.DersAtama).ThenInclude(da => da.Ders)
            .Include(od => od.DersAtama).ThenInclude(da => da.Donem)
            .Include(od => od.DersAtama).ThenInclude(da => da.Ogretmen)
            .Where(od => od.OgrenciId == ogrenciId)
            .OrderByDescending(od => od.DersAtama.Donem.Yil)
            .ThenBy(od => od.DersAtama.Ders.DersKodu)
            .ToListAsync();

    public async Task<IEnumerable<OgrenciDers>> GetByOgrenciAndDonemAsync(int ogrenciId, int donemId)
        => await _context.OgrenciDersler
            .Include(od => od.DersAtama).ThenInclude(da => da.Ders)
            .Include(od => od.DersAtama).ThenInclude(da => da.Ogretmen)
            .Where(od => od.OgrenciId == ogrenciId && od.DersAtama.DonemId == donemId)
            .OrderBy(od => od.DersAtama.Ders.DersKodu)
            .ToListAsync();

    public async Task<IEnumerable<OgrenciDers>> GetByDersAtamaIdAsync(int dersAtamaId)
        => await _context.OgrenciDersler
            .Include(od => od.Ogrenci).ThenInclude(o => o.Kullanici)
            .Where(od => od.DersAtamaId == dersAtamaId)
            .OrderBy(od => od.Ogrenci.OgrenciNo)
            .ToListAsync();

    public async Task<bool> IsAlreadyRegisteredAsync(int ogrenciId, int dersAtamaId)
        => await _context.OgrenciDersler
            .IgnoreQueryFilters()
            .AnyAsync(od => od.OgrenciId == ogrenciId
                         && od.DersAtamaId == dersAtamaId
                         && od.IsActive);

    public async Task<IEnumerable<OgrenciDers>> GetBekleyenTaleplerAsync(int? donemId = null)
        => await _context.OgrenciDersler
            .Include(od => od.Ogrenci).ThenInclude(o => o.Kullanici)
            .Include(od => od.Ogrenci).ThenInclude(o => o.Bolum)
            .Include(od => od.DersAtama).ThenInclude(da => da.Ders)
            .Include(od => od.DersAtama).ThenInclude(da => da.Donem)
            .Include(od => od.DersAtama).ThenInclude(da => da.Ogretmen)
            .Where(od => od.Durum == OgrenciDersDurum.Talep
                      && (donemId == null || od.DersAtama.DonemId == donemId))
            .OrderBy(od => od.KayitTarihi)
            .ToListAsync();

    public async Task<OgrenciDers?> GetWithDetailsAsync(int id)
        => await _context.OgrenciDersler
            .Include(od => od.Ogrenci).ThenInclude(o => o.Kullanici)
            .Include(od => od.Ogrenci).ThenInclude(o => o.Bolum)
            .Include(od => od.DersAtama).ThenInclude(da => da.Ders)
            .Include(od => od.DersAtama).ThenInclude(da => da.Donem)
            .Include(od => od.DersAtama).ThenInclude(da => da.Ogretmen)
            .FirstOrDefaultAsync(od => od.Id == id);

    public async Task<IEnumerable<OgrenciDers>> GetTranskriptAsync(int ogrenciId)
        => await _context.OgrenciDersler
            .IgnoreQueryFilters()
            .Include(od => od.DersAtama).ThenInclude(da => da.Ders)
            .Include(od => od.DersAtama).ThenInclude(da => da.Donem)
            .Where(od => od.OgrenciId == ogrenciId
                      && od.Durum != OgrenciDersDurum.Talep
                      && od.Durum != OgrenciDersDurum.Reddedildi)
            .OrderByDescending(od => od.DersAtama.Donem.Yil)
            .ThenBy(od => od.DersAtama.Ders.DersKodu)
            .ToListAsync();
}
