using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Repositories;

public class KullaniciRepository : GenericRepository<Kullanici>, IKullaniciRepository
{
    public KullaniciRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Kullanici?> GetByKullaniciAdiAsync(string kullaniciAdi)
        => await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.KullaniciAdi == kullaniciAdi);

    public async Task<Kullanici?> GetByEmailAsync(string email)
        => await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.Email == email);

    public async Task<Kullanici?> GetWithOgrenciAsync(int id)
        => await _context.Kullanicilar
            .Include(k => k.Ogrenci).ThenInclude(o => o!.Bolum)
            .FirstOrDefaultAsync(k => k.Id == id);

    public async Task<bool> IsKullaniciAdiUniqueAsync(string kullaniciAdi, int? excludeId = null)
        => !await _context.Kullanicilar
            .AnyAsync(k => k.KullaniciAdi == kullaniciAdi && (excludeId == null || k.Id != excludeId));

    public async Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
        => !await _context.Kullanicilar
            .AnyAsync(k => k.Email == email && (excludeId == null || k.Id != excludeId));

    public async Task<IEnumerable<Kullanici>> GetByRolAsync(KullaniciRol rol)
        => await _context.Kullanicilar
            .Include(k => k.Bolum)
            .Where(k => k.Rol == rol)
            .OrderBy(k => k.Soyad)
            .ThenBy(k => k.Ad)
            .ToListAsync();

    public async Task<IEnumerable<Kullanici>> GetOgretmenlerByBolumAsync(int bolumId)
        => await _context.Kullanicilar
            .Where(k => k.Rol == KullaniciRol.Ogretmen && k.BolumId == bolumId)
            .OrderBy(k => k.Soyad)
            .ToListAsync();
}
