using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;
using StudentManagement.Core.Interfaces.Repositories;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Repositories;

public class BelgeTalebiRepository : GenericRepository<BelgeTalebi>, IBelgeTalebiRepository
{
    public BelgeTalebiRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<BelgeTalebi>> GetByOgrenciAsync(int ogrenciId)
        => await _context.BelgeTalepleri
            .Where(b => b.OgrenciId == ogrenciId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<BelgeTalebi>> GetByDurumAsync(BelgeDurum durum)
        => await _context.BelgeTalepleri
            .Include(b => b.Ogrenci).ThenInclude(o => o.Kullanici)
            .Include(b => b.Ogrenci).ThenInclude(o => o.Bolum)
            .Where(b => b.Durum == durum)
            .OrderBy(b => b.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<BelgeTalebi>> GetAllWithDetailsAsync()
        => await _context.BelgeTalepleri
            .Include(b => b.Ogrenci).ThenInclude(o => o.Kullanici)
            .Include(b => b.Ogrenci).ThenInclude(o => o.Bolum)
            .Include(b => b.IslemYapan)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

    public async Task<BelgeTalebi?> GetWithDetailsAsync(int id)
        => await _context.BelgeTalepleri
            .Include(b => b.Ogrenci).ThenInclude(o => o.Kullanici)
            .Include(b => b.Ogrenci).ThenInclude(o => o.Bolum)
            .Include(b => b.IslemYapan)
            .FirstOrDefaultAsync(b => b.Id == id);
}
