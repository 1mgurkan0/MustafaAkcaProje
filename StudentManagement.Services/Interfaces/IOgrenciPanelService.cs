
namespace StudentManagement.Services.Interfaces
{
    public interface IOgrenciPanelService
    {
        Task<ServiceResult> BelgeTalepOlusturAsync(BelgeTalebiOlusturViewModel model, int ogrenciId, int userId);
        Task<ServiceResult> DersBirakAsync(int dersAtamaId, int ogrenciId, int userId);
        Task<ServiceResult> DersKayitTalepAsync(int dersAtamaId, int ogrenciId, int userId);
        Task<BelgelerViewModel> GetBelgelerAsync(int ogrenciId);
        Task<OgrenciDashboardViewModel> GetDashboardAsync(int ogrenciId);
        Task<DersKayitViewModel> GetDersKayitAsync(int ogrenciId);
        Task<IEnumerable<OgrenciDersViewModel>> GetDerslerimAsync(int ogrenciId);
        Task<DersProgramiViewModel> GetDersProgramiAsync(int ogrenciId);
        Task<IEnumerable<DuyuruOlusturViewModel>> GetDuyurularAsync(int ogrenciId, int bolumId);
        Task<SinavTakvimiViewModel> GetSinavTakvimiAsync(int ogrenciId);
        Task<TranskriptViewModel> GetTranskriptAsync(int ogrenciId);
    }
}