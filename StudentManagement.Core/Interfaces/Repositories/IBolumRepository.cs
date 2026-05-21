using StudentManagement.Core.Entities;

namespace StudentManagement.Core.Interfaces.Repositories;

public interface IBolumRepository : IGenericRepository<Bolum>
{
    Task<Bolum?> GetByKodAsync(string bolumKodu);
    Task<bool> IsBolumKoduUniqueAsync(string bolumKodu, int? excludeId = null);
    Task<IEnumerable<Bolum>> GetAllWithDerslerAsync();
    Task<Bolum?> GetWithOgrencilerAsync(int id);
}
