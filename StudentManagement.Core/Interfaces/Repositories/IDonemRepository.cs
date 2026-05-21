using StudentManagement.Core.Entities;

namespace StudentManagement.Core.Interfaces.Repositories;

public interface IDonemRepository : IGenericRepository<Donem>
{
    Task<Donem?> GetAktifDonemAsync();
    Task<Donem?> GetByKodAsync(string donemKodu);
    Task<IEnumerable<Donem>> GetAllOrderedAsync();
    Task<bool> IsDonemKoduUniqueAsync(string donemKodu, int? excludeId = null);

    /// <summary>
    /// Sistemdeki tüm aktif dönem işaretini kaldırır.
    /// Yeni aktif dönem seçilmeden önce çağrılır.
    /// </summary>
    Task ClearAktifDonemAsync();
}
