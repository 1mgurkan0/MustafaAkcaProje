
namespace StudentManagement.Services.Interfaces
{
    public interface IAdminService
    {
        Task<ServiceResult> BolumGuncelleAsync(BolumDuzenleViewModel model, int userId);
        Task<ServiceResult> BolumOlusturAsync(BolumOlusturViewModel model, int userId);
        Task<ServiceResult> BolumSilAsync(int id, int userId);
        Task<ServiceResult> DersAtamaOlusturAsync(DersAtamaOlusturViewModel model, int userId);
        Task<ServiceResult> DersAtamaSilAsync(int id, int userId);
        Task<ServiceResult> DersGuncelleAsync(DersDuzenleViewModel model, int userId);
        Task<ServiceResult> DersOlusturAsync(DersOlusturViewModel model, int userId);
        Task<ServiceResult> DersSilAsync(int id, int userId);
        Task<ServiceResult> DonemAktifYapAsync(int id, int userId);
        Task<ServiceResult> DonemGuncelleAsync(DonemDuzenleViewModel model, int userId);
        Task<ServiceResult> DonemOlusturAsync(DonemOlusturViewModel model, int userId);
        Task<AuditLogListViewModel> GetAuditLogsAsync(int page, int pageSize);
        Task<ServiceResult<BolumDuzenleViewModel>> GetBolumDuzenleAsync(int id);
        Task<IEnumerable<BolumViewModel>> GetBolumlerAsync();
        Task<IEnumerable<BolumSelectViewModel>> GetBolumSelectListAsync();
        Task<AdminDashboardViewModel> GetDashboardAsync();
        Task<ServiceResult<DersAtamaDetayViewModel>> GetDersAtamaDetayAsync(int id);
        Task<IEnumerable<DersAtamaViewModel>> GetDersAtamalarAsync();
        Task<ServiceResult<DersDuzenleViewModel>> GetDersDuzenleAsync(int id);
        Task<IEnumerable<DersViewModel>> GetDersKataloguAsync();
        Task<IEnumerable<DersSelectViewModel>> GetDersSelectListAsync();
        Task<ServiceResult<DonemDuzenleViewModel>> GetDonemDuzenleAsync(int id);
        Task<IEnumerable<DonemViewModel>> GetDonemlerAsync();
        Task<IEnumerable<DonemSelectViewModel>> GetDonemSelectListAsync();
        Task<IEnumerable<OgrenciViewModel>> GetOgrencilerAsync();
        Task<IEnumerable<KullaniciOzetViewModel>> GetOgretmenSelectListAsync();
        Task<ServiceResult> OgrenciDurumGuncelleAsync(int ogrenciId, OgrenciDurum yeniDurum, int userId);
    }
}