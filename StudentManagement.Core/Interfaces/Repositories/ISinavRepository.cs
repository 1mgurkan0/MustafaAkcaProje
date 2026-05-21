using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Interfaces.Repositories;

public interface ISinavRepository : IGenericRepository<Sinav>
{
    /// <summary>Bir ders atamasının tüm sınavları</summary>
    Task<IEnumerable<Sinav>> GetByDersAtamaAsync(int dersAtamaId);

    /// <summary>Öğrencinin kayıtlı olduğu derslerin sınavları (takvim için)</summary>
    Task<IEnumerable<Sinav>> GetByOgrenciAsync(int ogrenciId, int donemId);

    /// <summary>Öğretmenin derslerinin sınavları</summary>
    Task<IEnumerable<Sinav>> GetByOgretmenAsync(int ogretmenId, int donemId);

    /// <summary>Belirli tarih aralığındaki sınavlar (tüm sistem)</summary>
    Task<IEnumerable<Sinav>> GetByTarihAraligiAsync(DateTime baslangic, DateTime bitis);
}
