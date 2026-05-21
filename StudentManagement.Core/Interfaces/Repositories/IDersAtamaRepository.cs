using StudentManagement.Core.Entities;

namespace StudentManagement.Core.Interfaces.Repositories;

public interface IDersAtamaRepository : IGenericRepository<DersAtama>
{
    /// <summary>Bir öğretmenin dönemdeki tüm ders atamaları (Ders + Donem include)</summary>
    Task<IEnumerable<DersAtama>> GetByOgretmenAndDonemAsync(int ogretmenId, int donemId);

    /// <summary>Bir dönemdeki tüm ders atamaları</summary>
    Task<IEnumerable<DersAtama>> GetByDonemAsync(int donemId);

    /// <summary>Belirli bir bölümün dönemdeki ders atamaları (öğrenci kayıt için)</summary>
    Task<IEnumerable<DersAtama>> GetByBolumAndDonemAsync(int bolumId, int donemId);

    /// <summary>Detaylı: Ders + Donem + Öğretmen + OgrenciDersler sayısı</summary>
    Task<DersAtama?> GetWithDetailsAsync(int id);

    /// <summary>Aynı dönemde aynı ders zaten atanmış mı kontrolü</summary>
    Task<bool> IsAlreadyAtanmisAsync(int dersId, int donemId, int? excludeId = null);

    /// <summary>Kontenjan doluluk kontrolü</summary>
    Task<bool> IsKontenjanDoluAsync(int dersAtamaId);
}
