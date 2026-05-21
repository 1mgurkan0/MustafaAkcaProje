using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Repositories;

public class DuyuruRepository : GenericRepository<Duyuru>, IDuyuruRepository
{
    public DuyuruRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Duyuru>> GetForOgrenciAsync(int ogrenciId, int bolumId)
    {
        // Öğrencinin kayıtlı olduğu aktif DersAtama ID'leri
        var dersAtamaIds = await _context.OgrenciDersler
            .Where(od => od.OgrenciId == ogrenciId
                      && od.Durum == OgrenciDersDurum.Devam)
            .Select(od => od.DersAtamaId)
            .ToListAsync();

        return await _context.Duyurular
            .Include(d => d.Yayinlayan)
            .Where(d => d.Hedef == DuyuruHedef.Herkes
                     || d.Hedef == DuyuruHedef.Ogrenciler
                     || (d.Hedef == DuyuruHedef.BolumOzeli && d.HedefBolumId == bolumId)
                     || (d.Hedef == DuyuruHedef.DersOzeli  && d.HedefDersAtamaId.HasValue
                                                            && dersAtamaIds.Contains(d.HedefDersAtamaId.Value)))
            .Where(d => d.YayinTarihi == null || d.YayinTarihi <= DateTime.UtcNow)
            .Where(d => d.BitisTarihi == null  || d.BitisTarihi >= DateTime.UtcNow)
            .OrderByDescending(d => d.Onemli)
            .ThenByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Duyuru>> GetForOgretmenAsync(int ogretmenId)
    {
        var dersAtamaIds = await _context.DersAtamalar
            .Where(da => da.OgretmenId == ogretmenId)
            .Select(da => da.Id)
            .ToListAsync();

        return await _context.Duyurular
            .Include(d => d.Yayinlayan)
            .Where(d => d.Hedef == DuyuruHedef.Herkes
                     || d.Hedef == DuyuruHedef.Ogretmenler
                     || d.Hedef == DuyuruHedef.OgrenciIsleri
                     || (d.Hedef == DuyuruHedef.DersOzeli && d.HedefDersAtamaId.HasValue
                                                           && dersAtamaIds.Contains(d.HedefDersAtamaId.Value)))
            .OrderByDescending(d => d.Onemli)
            .ThenByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Duyuru>> GetAllWithYayinlayanAsync()
        => await _context.Duyurular
            .Include(d => d.Yayinlayan)
            .Include(d => d.HedefBolum)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<Duyuru>> GetByDersAtamaAsync(int dersAtamaId)
        => await _context.Duyurular
            .Include(d => d.Yayinlayan)
            .Where(d => d.HedefDersAtamaId == dersAtamaId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<Duyuru>> GetAktifDuyurularAsync(DuyuruHedef? hedef = null)
    {
        var query = _context.Duyurular
            .Include(d => d.Yayinlayan)
            .Where(d => (d.YayinTarihi == null || d.YayinTarihi <= DateTime.UtcNow)
                     && (d.BitisTarihi  == null || d.BitisTarihi  >= DateTime.UtcNow));

        if (hedef.HasValue)
            query = query.Where(d => d.Hedef == hedef.Value);

        return await query
            .OrderByDescending(d => d.Onemli)
            .ThenByDescending(d => d.CreatedAt)
            .ToListAsync();
    }
}
