using StudentManagement.Core.Entities;

namespace StudentManagement.Core.Interfaces.Repositories;

public interface IOgrenciRepository : IGenericRepository<Ogrenci>
{
    Task<Ogrenci?> GetWithKullaniciAsync(int id);
    Task<Ogrenci?> GetWithDerslerAsync(int id);
    Task<Ogrenci?> GetByOgrenciNoAsync(string ogrenciNo);
    Task<bool> IsOgrenciNoUniqueAsync(string ogrenciNo, int? excludeId = null);
    Task<IEnumerable<Ogrenci>> GetAllWithKullaniciAsync();

    /// <summary>Belirli bir bölümdeki öğrenciler</summary>
    Task<IEnumerable<Ogrenci>> GetByBolumAsync(int bolumId);

    /// <summary>KullaniciId'ye göre öğrenci kaydı getir (session'dan gelen userId ile)</summary>
    Task<Ogrenci?> GetByKullaniciIdAsync(int kullaniciId);

    /// <summary>
    /// Bir sonraki OgrenciNo'yu üretmek için kullanılır.
    /// Örn: yıl bazlı "2024" + sıra no "001" → "2024001"
    /// </summary>
    Task<int> GetSonOgrenciSirasiAsync(int yil);
}
