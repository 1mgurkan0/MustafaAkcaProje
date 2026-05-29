using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.Enums;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.ViewModels.Ogretmen;
using StudentManagement.Web.Filters;

namespace StudentManagement.Web.Controllers;

[RoleAuthFilter(KullaniciRol.Ogretmen)]
public class OgretmenPanelController : BaseController
{
    private readonly IOgretmenPanelService _service;
    public OgretmenPanelController(IOgretmenPanelService service) => _service = service;

    public async Task<IActionResult> Dashboard() => View(await _service.GetDashboardAsync(CurrentUserId));

    // HATA VEREN METOT: PARAMETREYİ KALDIRDIK
    public async Task<IActionResult> Derslerim() => View(await _service.GetDerslerimAsync(CurrentUserId));

    public async Task<IActionResult> DersDetay(int id)
    {
        var result = await _service.GetDersDetayAsync(id, CurrentUserId);
        if (result == null || result.Data == null) { SetErrorMessage("Derse erişim yetkiniz yok."); return RedirectToAction(nameof(Derslerim)); }
        return View(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> NotGir(int dersAtamaId)
    {
        var result = await _service.GetNotGirAsync(dersAtamaId, CurrentUserId);
        if (result == null || result.Data == null) { SetErrorMessage("Erişim yetkiniz yok."); return RedirectToAction(nameof(Derslerim)); }
        return View(result.Data);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> NotGir(NotGirViewModel model)
    {
        try
        {
            var result = await _service.NotKaydetAsync(model, CurrentUserId);
            if (result.IsSuccess)
                SetSuccessMessage("Notlar kaydedildi.");
            else
                SetErrorMessage(result.Message);
            return RedirectToAction(nameof(DersDetay), new { id = model.DersAtamaId });
        }
        catch (Exception ex) { SetErrorMessage(ex.Message); return View(model); }
    }

    [HttpGet]
    public async Task<IActionResult> Yoklama(int dersAtamaId)
    {
        var result = await _service.GetYoklamaGirAsync(dersAtamaId, CurrentUserId);
        if (result == null || result.Data == null) { SetErrorMessage("Erişim yetkiniz yok."); return RedirectToAction(nameof(Derslerim)); }
        return View(result.Data);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Yoklama(YoklamaGirViewModel model)
    {
        try
        {
            var result = await _service.YoklamaKaydetAsync(model, CurrentUserId);
            if (result.IsSuccess)
                SetSuccessMessage("Yoklama kaydedildi.");
            else
                SetErrorMessage(result.Message);
            return RedirectToAction(nameof(DersDetay), new { id = model.DersAtamaId });
        }
        catch (Exception ex) { SetErrorMessage(ex.Message); return View(model); }
    }

    public async Task<IActionResult> Sinavlar() => View(await _service.GetSinavlarAsync(CurrentUserId));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SinavEkle(SinavEkleViewModel model)
    {
        try { await _service.SinavEkleAsync(model, CurrentUserId); SetSuccessMessage("Sınav eklendi."); }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(DersDetay), new { id = model.DersAtamaId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SinavSil(int id, int dersAtamaId)
    {
        try { await _service.SinavSilAsync(id, CurrentUserId); SetSuccessMessage("Sınav silindi."); }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(DersDetay), new { id = dersAtamaId });
    }

    public async Task<IActionResult> Duyurular() => View(await _service.GetDuyurularAsync(CurrentUserId));
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DuyuruYayinla(DuyuruOlusturViewModel model)
    {
        try { await _service.DuyuruYayinlaAsync(model, CurrentUserId); SetSuccessMessage("Duyuru yayınlandı."); }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(DersDetay), new { id = model.HedefDersAtamaId });
    }
}