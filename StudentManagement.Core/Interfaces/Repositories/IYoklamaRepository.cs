using StudentManagement.Core.Entities;

namespace StudentManagement.Core.Interfaces.Repositories;

public interface IYoklamaRepository : IGenericRepository<Yoklama>
{
    /// <summary>Bir ders atamasının tüm yoklama oturumları</summary>
    Task<IEnumerable<Yoklama>> GetByDersAtamaAsync(int dersAtamaId);

    /// <summary>Aynı gün aynı ders için zaten yoklama alındı mı?</summary>
    Task<bool> IsYoklamaAlindıMiAsync(int dersAtamaId, DateTime tarih);

    /// <summary>Yoklama + OgrenciYoklama detayları</summary>
    Task<Yoklama?> GetWithOgrenciYoklamalarAsync(int yoklamaId);

    /// <summary>
    /// Bir öğrencinin bir ders atamasındaki devam özeti:
    /// toplam oturum, geldiği, gelmediği sayısı
    /// </summary>
    Task<(int Toplam, int Geldi, int Gelmedi)> GetDevamOzetiAsync(int ogrenciId, int dersAtamaId);
}
