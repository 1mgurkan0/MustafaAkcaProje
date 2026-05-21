using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Interfaces.Repositories;

public interface IBelgeTalebiRepository : IGenericRepository<BelgeTalebi>
{
    /// <summary>Bir öğrencinin tüm belge talepleri</summary>
    Task<IEnumerable<BelgeTalebi>> GetByOgrenciAsync(int ogrenciId);

    /// <summary>Öğrenci işleri paneli: duruma göre filtreli liste</summary>
    Task<IEnumerable<BelgeTalebi>> GetByDurumAsync(BelgeDurum durum);

    /// <summary>Öğrenci + Kullanici bilgisiyle detaylı liste</summary>
    Task<IEnumerable<BelgeTalebi>> GetAllWithDetailsAsync();

    Task<BelgeTalebi?> GetWithDetailsAsync(int id);
}
