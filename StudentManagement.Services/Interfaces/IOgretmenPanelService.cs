using StudentManagement.Services.ViewModels.Ogretmen;
using StudentManagement.Services.Helpers;

namespace StudentManagement.Services.Interfaces;

public interface IOgretmenPanelService
{
    Task<ServiceResult> DuyuruYayinlaAsync(DuyuruOlusturViewModel model, int ogretmenId);
    Task<OgretmenDashboardViewModel> GetDashboardAsync(int ogretmenId);
    Task<IEnumerable<DersSelectViewModel>> GetDersAtamaSelectListAsync(int ogretmenId);
    Task<ServiceResult<DersDetayViewModel>> GetDersDetayAsync(int dersAtamaId, int ogretmenId);
    Task<IEnumerable<DersAtamaOzetViewModel>> GetDerslerimAsync(int ogretmenId);
    Task<IEnumerable<DuyuruOlusturViewModel>> GetDuyurularAsync(int ogretmenId);
    Task<ServiceResult<NotGirViewModel>> GetNotGirAsync(int dersAtamaId, int ogretmenId);
    Task<SinavlarViewModel> GetSinavlarAsync(int ogretmenId);
    Task<ServiceResult<YoklamaGirViewModel>> GetYoklamaGirAsync(int dersAtamaId, int ogretmenId);
    Task<ServiceResult> NotKaydetAsync(NotGirViewModel model, int ogretmenId);
    Task<ServiceResult> SinavEkleAsync(SinavEkleViewModel model, int ogretmenId);
    Task<ServiceResult> SinavSilAsync(int id, int ogretmenId);
    Task<ServiceResult> YoklamaKaydetAsync(YoklamaGirViewModel model, int ogretmenId);
}