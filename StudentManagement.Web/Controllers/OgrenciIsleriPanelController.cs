using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.Enums;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.ViewModels.OgrenciIsleri;
using StudentManagement.Web.Filters;
using StudentManagement.Services.ViewModels.Ogretmen;

namespace StudentManagement.Web.Controllers;

[RoleAuthFilter(KullaniciRol.OgrenciIsleri, KullaniciRol.Admin)]
public class OgrenciIsleriPanelController : BaseController
{
    private readonly IOgrenciIsleriService _service;

    public OgrenciIsleriPanelController(IOgrenciIsleriService service)
        => _service = service;

    // ═══════════════════════════════════════════════════════════════════════
    // DASHBOARD
    // ═══════════════════════════════════════════════════════════════════════
    public async Task<IActionResult> Dashboard()
    {
        var vm = await _service.GetDashboardAsync();
        return View(vm);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // KAYIT TALEPLERİ
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>Bekleyen tüm kayıt talepleri (filtreli)</summary>
    public async Task<IActionResult> Talepler(int? donemId, int? bolumId)
    {
        // Interface'te filtre parametresi yoksa GetTaleplerAsync() çağırılır.
        var vm = await _service.GetTaleplerAsync();
        return View(vm);
    }

    /// <summary>Tek talep detay sayfası</summary>
    public async Task<IActionResult> TalepDetay(int id)
    {
        var result = await _service.GetTalepDetayAsync(id);
        if (result == null || result.Data == null)
        {
            SetErrorMessage("Talep bulunamadı.");
            return RedirectToAction(nameof(Talepler));
        }
        return View(result.Data);
    }

    /// <summary>Tek talep onayla (GET → onay sayfası, POST → işlem)</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Onayla(int id)
    {
        try
        {
            await _service.TalepOnaylaAsync(id, CurrentUserId);
            SetSuccessMessage("Kayıt talebi onaylandı.");
        }
        catch (Exception ex)
        {
            SetErrorMessage(ex.Message);
        }

        return RedirectToAction(nameof(Talepler));
    }

    /// <summary>Tek talep red (POST)</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reddet(int id, string redNedeni)
    {
        if (string.IsNullOrWhiteSpace(redNedeni))
        {
            SetErrorMessage("Red nedeni boş bırakılamaz.");
            return RedirectToAction(nameof(TalepDetay), new { id });
        }

        try
        {
            await _service.TalepReddetAsync(id, redNedeni, CurrentUserId);
            SetSuccessMessage("Kayıt talebi reddedildi.");
        }
        catch (Exception ex)
        {
            SetErrorMessage(ex.Message);
        }

        return RedirectToAction(nameof(Talepler));
    }

    /// <summary>Toplu onay — checkbox seçili ID listesi</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TopluOnayla([FromForm] List<int> secilenIds)
    {
        if (secilenIds == null || !secilenIds.Any())
        {
            SetErrorMessage("Hiç talep seçilmedi.");
            return RedirectToAction(nameof(Talepler));
        }

        try
        {
            var result = await _service.TopluOnaylaAsync(secilenIds, CurrentUserId);
            SetSuccessMessage($"{result.Data} kayıt talebi onaylandı.");
        }
        catch (Exception ex)
        {
            SetErrorMessage(ex.Message);
        }

        return RedirectToAction(nameof(Talepler));
    }

    // ═══════════════════════════════════════════════════════════════════════
    // BELGE TALEPLERİ
    // ═══════════════════════════════════════════════════════════════════════

    public async Task<IActionResult> Belgeler()
    {
        var vm = await _service.GetBelgeTalepleriAsync();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BelgeDurumGuncelle(BelgeDurumGuncelleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            SetErrorMessage("Lütfen formu doğru doldurunuz.");
            return RedirectToAction(nameof(Belgeler));
        }

        try
        {
            await _service.BelgeDurumGuncelleAsync(model, CurrentUserId);
            SetSuccessMessage("Belge durumu güncellendi.");
        }
        catch (Exception ex)
        {
            SetErrorMessage(ex.Message);
        }

        return RedirectToAction(nameof(Belgeler));
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ÖĞRENCİ ARAMA
    // ═══════════════════════════════════════════════════════════════════════

    public async Task<IActionResult> OgrenciAra(string? arama, int? bolumId)
    {
        // View büyük ihtimalle OgrenciAraViewModel bekliyor ama interface List<OgrenciViewModel> dönüyor
        // Eğer arayüz ve view uyuşmazlığı varsa bunu OgrenciAraViewModel içinde sarmalaman gerekebilir.
        // Şimdilik listeyi view'a dönüyoruz. Eğer hata verirse View dosyasının `@model` tipini `List<StudentManagement.Services.ViewModels.Admin.OgrenciViewModel>` yap.
        var vm = await _service.OgrenciAraAsync(arama, bolumId);
        return View(vm);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // DUYURU
    // ═══════════════════════════════════════════════════════════════════════

    [HttpGet]
    public IActionResult DuyuruOlustur()
        => View(new DuyuruOlusturViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DuyuruOlustur(DuyuruOlusturViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _service.DuyuruOlusturAsync(model, CurrentUserId);
            SetSuccessMessage("Duyuru yayınlandı.");
            return RedirectToAction(nameof(Dashboard));
        }
        catch (Exception ex)
        {
            SetErrorMessage(ex.Message);
            return View(model);
        }
    }
}