using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.Enums;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.ViewModels.Ogrenci;
using StudentManagement.Web.Filters;
using StudentManagement.Services.ViewModels.OgrenciIsleri; // Belge talebi için eklendi

namespace StudentManagement.Web.Controllers;

[RoleAuthFilter(KullaniciRol.Ogrenci)]
public class OgrenciPanelController : BaseController
{
    private readonly IOgrenciPanelService _service;

    public OgrenciPanelController(IOgrenciPanelService service) => _service = service;

    public async Task<IActionResult> Dashboard()
        => View(await _service.GetDashboardAsync(CurrentUserId));

    public async Task<IActionResult> DersKayit()
    {
        var vm = await _service.GetDersKayitAsync(CurrentUserId);
        if (vm == null) { SetErrorMessage("Öğrenci kaydı bulunamadı."); return RedirectToAction(nameof(Dashboard)); }
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DersKayitTalep(int dersAtamaId)
    {
        try
        {
            await _service.DersKayitTalepAsync(CurrentUserId);
            SetSuccessMessage("Ders kayıt talebiniz iletildi. Öğrenci İşleri onayından sonra aktif olacak.");
        }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(DersKayit));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DersBirak(int ogrenciDersId)
    {
        try
        {
            await _service.DersBirakAsync(CurrentUserId);
            SetSuccessMessage("Dersten çekilme işlemi tamamlandı.");
        }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(Derslerim));
    }

    public async Task<IActionResult> Derslerim()
    {
        var vm = await _service.GetDashboardAsync(CurrentUserId);
        return View(vm.BuDonemDersler);
    }

    public async Task<IActionResult> Transkript()
    {
        var vm = await _service.GetTranskriptAsync(CurrentUserId);
        if (vm == null) { SetErrorMessage("Transkript bilgisi bulunamadı."); return RedirectToAction(nameof(Dashboard)); }
        return View(vm);
    }

    public async Task<IActionResult> DersProgrami()
    {
        var vm = await _service.GetDersProgramiAsync(CurrentUserId);
        if (vm == null) { SetErrorMessage("Aktif dönem bulunamadı."); return RedirectToAction(nameof(Dashboard)); }
        return View(vm);
    }

    public async Task<IActionResult> SinavTakvimi()
    {
        var vm = await _service.GetSinavTakvimiAsync(CurrentUserId);
        if (vm == null) { SetErrorMessage("Aktif dönem bulunamadı."); return RedirectToAction(nameof(Dashboard)); }
        return View(vm);
    }

    public async Task<IActionResult> Duyurular()
        => View(await _service.GetDuyurularAsync(CurrentUserId, 0)); // Parametre eksiği giderildi

    public async Task<IActionResult> Belgeler()
        => View(await _service.GetBelgeTaleplerAsync(CurrentUserId));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> BelgeTalepOlustur(BelgeTalebiOlusturViewModel model) // Parametre tipi düzeltildi
    {
        try
        {
            // Arayüzün beklediği 3 parametre (model, userId, bolumId) gönderildi
            await _service.BelgeTalepOlusturAsync(model, CurrentUserId, 0);
            SetSuccessMessage("Belge talebiniz oluşturuldu.");
        }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(Belgeler));
    }
}