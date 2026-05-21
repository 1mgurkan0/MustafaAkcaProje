using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Repositories;

public class OgrenciRepository : GenericRepository<Ogrenci>, IOgrenciRepository
{
    public OgrenciRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Ogrenci?> GetWithKullaniciAsync(int id)
        => await _context.Ogrenciler
            .Include(o => o.Kullanici)
            .Include(o => o.Bolum)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Ogrenci?> GetWithDerslerAsync(int id)
        => await _context.Ogrenciler
            .Include(o => o.Kullanici)
            .Include(o => o.Bolum)
            .Include(o => o.OgrenciDersler)
                .ThenInclude(od => od.DersAtama)
                .ThenInclude(da => da.Ders)
            .Include(o => o.OgrenciDersler)
                .ThenInclude(od => od.DersAtama)
                .ThenInclude(da => da.Donem)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Ogrenci?> GetByOgrenciNoAsync(string ogrenciNo)
        => await _context.Ogrenciler
            .Include(o => o.Kullanici)
            .Include(o => o.Bolum)
            .FirstOrDefaultAsync(o => o.OgrenciNo == ogrenciNo);

    public async Task<bool> IsOgrenciNoUniqueAsync(string ogrenciNo, int? excludeId = null)
        => !await _context.Ogrenciler
            .AnyAsync(o => o.OgrenciNo == ogrenciNo && (excludeId == null || o.Id != excludeId));

    public async Task<IEnumerable<Ogrenci>> GetAllWithKullaniciAsync()
        => await _context.Ogrenciler
            .Include(o => o.Kullanici)
            .Include(o => o.Bolum)
            .OrderBy(o => o.OgrenciNo)
            .ToListAsync();

    public async Task<IEnumerable<Ogrenci>> GetByBolumAsync(int bolumId)
        => await _context.Ogrenciler
            .Include(o => o.Kullanici)
            .Where(o => o.BolumId == bolumId)
            .OrderBy(o => o.OgrenciNo)
            .ToListAsync();

    public async Task<Ogrenci?> GetByKullaniciIdAsync(int kullaniciId)
        => await _context.Ogrenciler
            .Include(o => o.Kullanici)
            .Include(o => o.Bolum)
            .Include(o => o.AktifDonem)
            .FirstOrDefaultAsync(o => o.KullaniciId == kullaniciId);

    public async Task<int> GetSonOgrenciSirasiAsync(int yil)
    {
        // "2024001" → "2024" prefix ile başlayan kayıtların sayısını döner
        var prefix = yil.ToString();
        return await _context.Ogrenciler
            .IgnoreQueryFilters()
            .CountAsync(o => o.OgrenciNo.StartsWith(prefix));
    }
}
