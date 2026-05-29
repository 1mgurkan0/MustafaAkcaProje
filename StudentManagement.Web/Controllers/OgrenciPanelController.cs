using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.Enums;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.ViewModels.Ogrenci;
using StudentManagement.Web.Filters;

namespace StudentManagement.Web.Controllers;

[RoleAuthFilter(KullaniciRol.Ogrenci)]
public class OgrenciPanelController : BaseController
{
    private readonly IOgrenciPanelService _service;

    public OgrenciPanelController(IOgrenciPanelService service) => _service = service;

    public async Task<IActionResult> Dashboard()
        => View(await _service.GetDashboardAsync(CurrentOgrenciId));

    public async Task<IActionResult> DersKayit()
    {
        var vm = await _service.GetDersKayitAsync(CurrentOgrenciId);
        if (vm == null) { SetErrorMessage("Öğrenci kaydı bulunamadı."); return RedirectToAction(nameof(Dashboard)); }
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DersKayitTalep(int dersAtamaId)
    {
        try
        {
            var result = await _service.DersKayitTalepAsync(dersAtamaId, CurrentOgrenciId, CurrentUserId);
            if (result.IsSuccess)
                SetSuccessMessage("Ders kayıt talebiniz iletildi. Öğrenci İşleri onayından sonra aktif olacak.");
            else
                SetErrorMessage(result.Message);
        }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(DersKayit));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DersBirak(int ogrenciDersId)
    {
        try
        {
            var result = await _service.DersBirakAsync(ogrenciDersId, CurrentOgrenciId, CurrentUserId);
            if (result.IsSuccess)
                SetSuccessMessage("Dersten çekilme işlemi tamamlandı.");
            else
                SetErrorMessage(result.Message);
        }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(Derslerim));
    }

    public async Task<IActionResult> Derslerim()
        => View(await _service.GetDerslerimAsync(CurrentOgrenciId));

    public async Task<IActionResult> Transkript()
    {
        var vm = await _service.GetTranskriptAsync(CurrentOgrenciId);
        if (vm == null) { SetErrorMessage("Transkript bilgisi bulunamadı."); return RedirectToAction(nameof(Dashboard)); }
        return View(vm);
    }

    public async Task<IActionResult> DersProgrami()
    {
        var vm = await _service.GetDersProgramiAsync(CurrentOgrenciId);
        if (vm == null) { SetErrorMessage("Aktif dönem bulunamadı."); return RedirectToAction(nameof(Dashboard)); }
        return View(vm);
    }

    public async Task<IActionResult> SinavTakvimi()
    {
        var vm = await _service.GetSinavTakvimiAsync(CurrentOgrenciId);
        if (vm == null) { SetErrorMessage("Aktif dönem bulunamadı."); return RedirectToAction(nameof(Dashboard)); }
        return View(vm);
    }

    public async Task<IActionResult> Duyurular()
        => View(await _service.GetDuyurularAsync(CurrentOgrenciId, CurrentBolumId));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DuyuruOkunduIsaretle(int duyuruId)
    {
        var result = await _service.DuyuruOkunduIsaretle(duyuruId, CurrentOgrenciId);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(new { success = result.IsSuccess, message = result.Message });

        if (result.IsSuccess)
            SetSuccessMessage("Duyuru okundu olarak işaretlendi.");
        else
            SetErrorMessage(result.Message);
        return RedirectToAction(nameof(Duyurular));
    }


    public async Task<IActionResult> Belgeler() => View(await _service.GetBelgelerAsync(CurrentOgrenciId));

    public async Task<IActionResult> Profil()
    {
        var dashboard = await _service.GetDashboardAsync(CurrentOgrenciId);
        return View(dashboard);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> BelgeTalepOlustur(BelgeTalebiOlusturViewModel model)
    {
        try
        {
            var result = await _service.BelgeTalepOlusturAsync(model, CurrentOgrenciId, CurrentUserId);
            if (result.IsSuccess)
                SetSuccessMessage("Belge talebiniz oluşturuldu.");
            else
                SetErrorMessage(result.Message);
        }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(Belgeler));
    }
}