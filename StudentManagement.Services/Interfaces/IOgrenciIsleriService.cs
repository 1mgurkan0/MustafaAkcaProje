
using StudentManagement.Services.ViewModels.OgrenciIsleri;

namespace StudentManagement.Services.Interfaces
{
    public interface IOgrenciIsleriService
    {
        Task<ServiceResult> BelgeDurumGuncelleAsync(BelgeDurumGuncelleViewModel model, int userId);
        Task<ServiceResult> DuyuruOlusturAsync(DuyuruOlusturViewModel model, int userId);
        Task<IEnumerable<ViewModels.OgrenciIsleri.BelgeTalebiViewModel>> GetBelgeTalepleriAsync();
        Task<IEnumerable<BolumSelectViewModel>> GetBolumSelectListAsync();
        Task<OgrenciIsleriDashboardViewModel> GetDashboardAsync();
        Task<ServiceResult<TalepDetayViewModel>> GetTalepDetayAsync(int ogrenciDersId);
        Task<IEnumerable<TalepDetayViewModel>> GetTaleplerAsync();
        Task<List<OgrenciIsleriOgrenciViewModel>> OgrenciAraAsync(string? q, int? bolumId);
        Task<ServiceResult<ViewModels.Admin.OgrenciDetayViewModel>> OgrenciDetayAsync(int ogrenciId);
        Task<ServiceResult> TalepOnaylaAsync(int ogrenciDersId, int userId);
        Task<ServiceResult> TalepReddetAsync(int ogrenciDersId, string redNedeni, int userId);
        Task<ServiceResult<int>> TopluOnaylaAsync(List<int> idler, int userId);
    }
}