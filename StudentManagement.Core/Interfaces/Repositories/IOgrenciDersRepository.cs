using StudentManagement.Core.Entities;

namespace StudentManagement.Core.Interfaces.Repositories;

public interface IOgrenciDersRepository : IGenericRepository<OgrenciDers>
{
    /// <summary>Öğrenci + DersAtama kombinasyonu unique kontrolü</summary>
    Task<OgrenciDers?> GetByOgrenciAndDersAtamaAsync(int ogrenciId, int dersAtamaId);

    /// <summary>Öğrencinin tüm dönemlerindeki ders kayıtları</summary>
    Task<IEnumerable<OgrenciDers>> GetByOgrenciIdAsync(int ogrenciId);

    /// <summary>Öğrencinin belirli bir dönemdeki kayıtları</summary>
    Task<IEnumerable<OgrenciDers>> GetByOgrenciAndDonemAsync(int ogrenciId, int donemId);

    /// <summary>Bir DersAtama'daki tüm öğrenci kayıtları (not girişi için)</summary>
    Task<IEnumerable<OgrenciDers>> GetByDersAtamaIdAsync(int dersAtamaId);

    /// <summary>
    /// Daha önce aynı ders atamasına kayıt var mı?
    /// IgnoreQueryFilters ile soft-delete'li kayıtları da kontrol eder.
    /// </summary>
    Task<bool> IsAlreadyRegisteredAsync(int ogrenciId, int dersAtamaId);

    /// <summary>Onay bekleyen talepler (Öğrenci İşleri paneli)</summary>
    Task<IEnumerable<OgrenciDers>> GetBekleyenTaleplerAsync(int? donemId = null);

    /// <summary>OgrenciDers + DersAtama + Ders + Donem + Öğretmen detaylı</summary>
    Task<OgrenciDers?> GetWithDetailsAsync(int id);

    /// <summary>
    /// Öğrencinin transkripti için tüm dönem kayıtları
    /// (IsActive filtresi kapalı — soft-delete'li eski kayıtlar da dahil)
    /// </summary>
    Task<IEnumerable<OgrenciDers>> GetTranskriptAsync(int ogrenciId);
}
