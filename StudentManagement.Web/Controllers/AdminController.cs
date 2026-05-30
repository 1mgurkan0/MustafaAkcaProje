using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentManagement.Core.Enums;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.ViewModels.Admin;
using StudentManagement.Web.Filters;

namespace StudentManagement.Web.Controllers;

// [RoleAuthFilter(KullaniciRol.Admin)]
public class AdminController : BaseController
{
    private readonly IAdminService _admin;
    public AdminController(IAdminService admin) => _admin = admin;

    // ── Dashboard ────────────────────────────────────────────────────────────
    public async Task<IActionResult> Dashboard()
        => View(await _admin.GetDashboardAsync());

    // ── Bölüm ────────────────────────────────────────────────────────────────
    public async Task<IActionResult> Bolumler()
        => View(await _admin.GetBolumlerAsync());

    [HttpGet]
    public IActionResult BolumOlustur() => View(new BolumOlusturViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> BolumOlustur(BolumOlusturViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        try { await _admin.BolumOlusturAsync(model, CurrentUserId); SetSuccessMessage("Bölüm oluşturuldu."); return RedirectToAction(nameof(Bolumler)); }
        catch (Exception ex) { SetErrorMessage(ex.Message); return View(model); }
    }

    [HttpGet]
    public async Task<IActionResult> BolumDuzenle(int id)
    {
        var result = await _admin.GetBolumDuzenleAsync(id);
        if (result == null || result.Data == null) { SetErrorMessage("Bölüm bulunamadı."); return RedirectToAction(nameof(Bolumler)); }
        return View(result.Data);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> BolumDuzenle(BolumDuzenleViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        try { await _admin.BolumGuncelleAsync(model, CurrentUserId); SetSuccessMessage("Bölüm güncellendi."); return RedirectToAction(nameof(Bolumler)); }
        catch (Exception ex) { SetErrorMessage(ex.Message); return View(model); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> BolumSil(int id)
    {
        try { await _admin.BolumSilAsync(id, CurrentUserId); SetSuccessMessage("Bölüm silindi."); }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(Bolumler));
    }

    // ── Dönem ────────────────────────────────────────────────────────────────
    public async Task<IActionResult> Donemler() => View(await _admin.GetDonemlerAsync());

    [HttpGet]
    public IActionResult DonemOlustur() => View(new DonemOlusturViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DonemOlustur(DonemOlusturViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _admin.DonemOlusturAsync(model, CurrentUserId);
        if (result.IsSuccess)
        {
            SetSuccessMessage("Dönem oluşturuldu.");
            return RedirectToAction(nameof(Donemler));
        }
        SetErrorMessage(result.Message);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> DonemDuzenle(int id)
    {
        var result = await _admin.GetDonemDuzenleAsync(id);
        if (result == null || result.Data == null) { SetErrorMessage("Dönem bulunamadı."); return RedirectToAction(nameof(Donemler)); }
        return View(result.Data);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DonemDuzenle(DonemDuzenleViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _admin.DonemGuncelleAsync(model, CurrentUserId);
        if (result.IsSuccess)
        {
            SetSuccessMessage("Dönem güncellendi.");
            return RedirectToAction(nameof(Donemler));
        }
        SetErrorMessage(result.Message);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DonemAktifYap(int id)
    {
        try { await _admin.DonemAktifYapAsync(id, CurrentUserId); SetSuccessMessage("Aktif dönem değiştirildi."); }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(Donemler));
    }

    // ── Ders Atama ────────────────────────────────────────────────────────────
    public async Task<IActionResult> DersAtamalar(int? donemId)
        => View(await _admin.GetDersAtamalarAsync());

    public async Task<IActionResult> DersAtamaDetay(int id)
    {
        var result = await _admin.GetDersAtamaDetayAsync(id);
        if (result == null || result.Data == null) { SetErrorMessage("Bulunamadı."); return RedirectToAction(nameof(DersAtamalar)); }
        return View(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> DersAtamaOlustur()
    {
        ViewBag.Dersler    = new SelectList(await _admin.GetDersSelectListAsync(), "Id", "DisplayText");
        ViewBag.Donemler   = new SelectList(await _admin.GetDonemSelectListAsync(), "Id", "DisplayText");
        ViewBag.Ogretmenler = new SelectList(await _admin.GetOgretmenSelectListAsync(), "Id", "TamAd");
        return View(new DersAtamaOlusturViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DersAtamaOlustur(DersAtamaOlusturViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Dersler    = new SelectList(await _admin.GetDersSelectListAsync(), "Id", "DisplayText");
            ViewBag.Donemler   = new SelectList(await _admin.GetDonemSelectListAsync(), "Id", "DisplayText");
            ViewBag.Ogretmenler = new SelectList(await _admin.GetOgretmenSelectListAsync(), "Id", "TamAd");
            return View(model);
        }

        var result = await _admin.DersAtamaOlusturAsync(model, CurrentUserId);
        if (result.IsSuccess)
        {
            SetSuccessMessage("Ders ataması oluşturuldu.");
            return RedirectToAction(nameof(DersAtamalar));
        }

        SetErrorMessage(result.Message);
        ViewBag.Dersler    = new SelectList(await _admin.GetDersSelectListAsync(), "Id", "DisplayText");
        ViewBag.Donemler   = new SelectList(await _admin.GetDonemSelectListAsync(), "Id", "DisplayText");
        ViewBag.Ogretmenler = new SelectList(await _admin.GetOgretmenSelectListAsync(), "Id", "TamAd");
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DersAtamaSil(int id)
    {
        try { await _admin.DersAtamaSilAsync(id, CurrentUserId); SetSuccessMessage("Ders ataması silindi."); }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(DersAtamalar));
    }

    // ════════════════════════════════════════════════════════════════════════
    // ÖĞRENCİLER
    // ════════════════════════════════════════════════════════════════════════
    public async Task<IActionResult> Ogrenciler(string? arama, int? bolumId, int? sinif)
        => View(await _admin.GetOgrencilerAsync()); // Interface parametre almadığı için içini boşalttık

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> OgrenciDurumGuncelle(AdminOgrenciDurumViewModel model)
    {
        // View modelindeki property ismine göre model.OgrenciId ve model.YeniDurum eşleşmeli
        try { await _admin.OgrenciDurumGuncelleAsync(model.OgrenciId, model.YeniDurum, CurrentUserId); SetSuccessMessage("Öğrenci durumu güncellendi."); }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(Ogrenciler));
    }

    [HttpGet]
    public async Task<IActionResult> OgrenciOlustur()
    {
        var model = new AdminOgrenciOlusturViewModel();
        await PopulateOgrenciOlusturDropdowns(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> OgrenciOlustur(AdminOgrenciOlusturViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateOgrenciOlusturDropdowns(model);
            return View(model);
        }

        var result = await _admin.OgrenciOlusturAsync(model, CurrentUserId);
        if (result.IsSuccess)
        {
            SetSuccessMessage("Öğrenci başarıyla oluşturuldu.");
            return RedirectToAction(nameof(Ogrenciler));
        }

        SetErrorMessage(result.Message);
        await PopulateOgrenciOlusturDropdowns(model);
        return View(model);
    }

    private async Task PopulateOgrenciOlusturDropdowns(AdminOgrenciOlusturViewModel model)
    {
        var bolumler = await _admin.GetBolumSelectListAsync();
        model.BolumListesi = bolumler.Select(b => new SelectListItem(b.DisplayText, b.Id.ToString())).ToList();
    }

    [HttpGet]
    public async Task<IActionResult> OgrenciGuncelle(int id)
    {
        var result = await _admin.GetOgrenciGuncelleAsync(id);
        if (!result.IsSuccess || result.Data == null)
        {
            SetErrorMessage(result.Message);
            return RedirectToAction(nameof(Ogrenciler));
        }

        await PopulateOgrenciGuncelleDropdowns(result.Data);
        return View(result.Data);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> OgrenciGuncelle(AdminOgrenciGuncelleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateOgrenciGuncelleDropdowns(model);
            return View(model);
        }

        var result = await _admin.OgrenciGuncelleAsync(model, CurrentUserId);
        if (result.IsSuccess)
        {
            SetSuccessMessage("Öğrenci başarıyla güncellendi.");
            return RedirectToAction(nameof(Ogrenciler));
        }

        SetErrorMessage(result.Message);
        await PopulateOgrenciGuncelleDropdowns(model);
        return View(model);
    }

    private async Task PopulateOgrenciGuncelleDropdowns(AdminOgrenciGuncelleViewModel model)
    {
        var bolumler = await _admin.GetBolumSelectListAsync();
        model.BolumListesi = bolumler.Select(b => new SelectListItem(b.DisplayText, b.Id.ToString())).ToList();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> OgrenciSil(int id)
    {
        var result = await _admin.OgrenciSilAsync(id, CurrentUserId);
        if (result.IsSuccess)
        {
            SetSuccessMessage("Öğrenci kalıcı olarak silindi.");
        }
        else
        {
            SetErrorMessage(result.Message);
        }
        return RedirectToAction(nameof(Ogrenciler));
    }

    // ════════════════════════════════════════════════════════════════════════
    // DERS KATALOĞU
    // ════════════════════════════════════════════════════════════════════════
    public async Task<IActionResult> DersKatalogu(string? arama, int? bolumId)
        => View(await _admin.GetDersKataloguAsync()); // Interface parametre almadığı için içini boşalttık

    [HttpGet]
    public async Task<IActionResult> DersOlustur()
    {
        ViewBag.Bolumler = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _admin.GetBolumSelectListAsync(), "Id", "DisplayText");
        return View(new DersOlusturViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DersOlustur(DersOlusturViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Bolumler = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _admin.GetBolumSelectListAsync(), "Id", "DisplayText");
            return View(model);
        }

        try
        {
            await _admin.DersOlusturAsync(model, CurrentUserId);
            SetSuccessMessage("Ders oluşturuldu.");
            return RedirectToAction(nameof(DersKatalogu));
        }
        catch (Exception ex)
        {
            SetErrorMessage(ex.Message);
            ViewBag.Bolumler = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _admin.GetBolumSelectListAsync(), "Id", "DisplayText");
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> DersDuzenle(int id)
    {
        var result = await _admin.GetDersDuzenleAsync(id);
        if (result == null || result.Data == null) { SetErrorMessage("Ders bulunamadı."); return RedirectToAction(nameof(DersKatalogu)); }
        return View(result.Data);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DersDuzenle(DersDuzenleViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        try { await _admin.DersGuncelleAsync(model, CurrentUserId); SetSuccessMessage("Ders güncellendi."); return RedirectToAction(nameof(DersKatalogu)); }
        catch (Exception ex) { SetErrorMessage(ex.Message); return View(model); }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DersSil(int id)
    {
        try { await _admin.DersSilAsync(id, CurrentUserId); SetSuccessMessage("Ders silindi."); }
        catch (Exception ex) { SetErrorMessage(ex.Message); }
        return RedirectToAction(nameof(DersKatalogu));
    }
}