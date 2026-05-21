using StudentManagement.Core.Entities;

namespace StudentManagement.Core.Interfaces.Repositories;

public interface IDersRepository : IGenericRepository<Ders>
{
    Task<Ders?> GetByDersKoduAsync(string dersKodu);
    Task<bool> IsDersKoduUniqueAsync(string dersKodu, int? excludeId = null);

    /// <summary>Belirli bir bölümün ders kataloğu</summary>
    Task<IEnumerable<Ders>> GetByBolumAsync(int bolumId);

    /// <summary>Ders + aktif dönem DersAtama + Öğretmen bilgisiyle</summary>
    Task<Ders?> GetWithAtamalarAsync(int id);

    /// <summary>Tüm dersler + bölüm adı (admin listesi)</summary>
    Task<IEnumerable<Ders>> GetAllWithBolumAsync();
}
