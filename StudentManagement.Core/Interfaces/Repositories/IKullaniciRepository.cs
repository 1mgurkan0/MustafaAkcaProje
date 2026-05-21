using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Interfaces.Repositories;

public interface IKullaniciRepository : IGenericRepository<Kullanici>
{
    Task<Kullanici?> GetByKullaniciAdiAsync(string kullaniciAdi);
    Task<Kullanici?> GetByEmailAsync(string email);
    Task<Kullanici?> GetWithOgrenciAsync(int id);
    Task<bool> IsKullaniciAdiUniqueAsync(string kullaniciAdi, int? excludeId = null);
    Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);

    /// <summary>Role göre kullanıcı listesi (admin paneli)</summary>
    Task<IEnumerable<Kullanici>> GetByRolAsync(KullaniciRol rol);

    /// <summary>Bir bölüme atanmış öğretmenler</summary>
    Task<IEnumerable<Kullanici>> GetOgretmenlerByBolumAsync(int bolumId);
}
