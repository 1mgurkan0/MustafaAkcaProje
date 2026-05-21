using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;

namespace StudentManagement.Core.Interfaces.Repositories;

public interface IDuyuruRepository : IGenericRepository<Duyuru>
{
    /// <summary>
    /// Bir öğrencinin görebileceği tüm duyurular:
    /// Herkes + Ogrenciler + kendi BolumOzeli + kayıtlı DersOzeli
    /// </summary>
    Task<IEnumerable<Duyuru>> GetForOgrenciAsync(int ogrenciId, int bolumId);

    /// <summary>Bir öğretmenin duyuruları (kendi ders atamaları + Herkes + Ogretmenler)</summary>
    Task<IEnumerable<Duyuru>> GetForOgretmenAsync(int ogretmenId);

    /// <summary>Admin / Öğrenci İşleri → tüm duyurular (yayınlayan filtresi opsiyonel)</summary>
    Task<IEnumerable<Duyuru>> GetAllWithYayinlayanAsync();

    /// <summary>Belirli bir ders atamasına ait duyurular</summary>
    Task<IEnumerable<Duyuru>> GetByDersAtamaAsync(int dersAtamaId);

    /// <summary>Aktif (yayın tarihi geçmemiş, bitiş tarihi dolmamış) duyurular</summary>
    Task<IEnumerable<Duyuru>> GetAktifDuyurularAsync(DuyuruHedef? hedef = null);
}
