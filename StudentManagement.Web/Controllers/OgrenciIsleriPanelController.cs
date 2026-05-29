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
        var dersTalepleri = await _service.GetTaleplerAsync();
        var belgeTalepleri = await _service.GetBelgeTalepleriAsync();

        var vm = new TumTaleplerViewModel
        {
            DersKayitTalepleri = dersTalepleri,
            BelgeTalepleri = belgeTalepleri
        };
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
    public async Task<IActionResult> Onayla(int ogrenciDersId)
    {
        try
        {
            await _service.TalepOnaylaAsync(ogrenciDersId, CurrentUserId);
            SetSuccessMessage("Kayıt talebi onaylandı.");
        }
        catch (Exception ex)
        {
            SetErrorMessage(ex.Message);
        }

        return RedirectToAction(nameof(Talepler));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reddet(int OgrenciDersId, string RedNedeni)
    {
        if (string.IsNullOrWhiteSpace(RedNedeni))
        {
            SetErrorMessage("Red nedeni boş bırakılamaz.");
            return RedirectToAction(nameof(TalepDetay), new { id = OgrenciDersId });
        }

        try
        {
            await _service.TalepReddetAsync(OgrenciDersId, RedNedeni, CurrentUserId);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BelgeYukle(int belgeTalebiId, Microsoft.AspNetCore.Http.IFormFile? belgeFile)
    {
        if (belgeFile == null || belgeFile.Length == 0)
        {
            SetErrorMessage("Lütfen bir dosya seçiniz.");
            return RedirectToAction(nameof(Belgeler));
        }

        try
        {
            var result = await _service.BelgeYukleAsync(belgeTalebiId, belgeFile, CurrentUserId);
            if (result.IsSuccess)
                SetSuccessMessage("Belge başarıyla yüklendi.");
            else
                SetErrorMessage(result.Message);
        }
        catch (Exception ex)
        {
            SetErrorMessage(ex.Message);
        }

        return RedirectToAction(nameof(Belgeler));
    }

    [HttpGet]
    public IActionResult BelgeIndir(string yol, string ad)
    {
        if (string.IsNullOrWhiteSpace(yol))
        {
            SetErrorMessage("Belge bulunamadı.");
            return RedirectToAction(nameof(Belgeler));
        }

        // wwwroot'a göreceli yol
        var tamYol = Path.Combine("wwwroot", yol.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (!System.IO.File.Exists(tamYol))
        {
            SetErrorMessage("Belge dosyası sunucuda bulunamadı.");
            return RedirectToAction(nameof(Belgeler));
        }

        var contentType = "application/octet-stream";
        var uzanti = Path.GetExtension(yol).ToLowerInvariant();
        if (uzanti == ".pdf") contentType = "application/pdf";
        else if (uzanti is ".jpg" or ".jpeg") contentType = "image/jpeg";
        else if (uzanti == ".png") contentType = "image/png";
        else if (uzanti is ".doc" or ".docx") contentType = "application/msword";

        var dosyaAdi = string.IsNullOrWhiteSpace(ad) ? Path.GetFileName(yol) : ad;
        return PhysicalFile(Path.GetFullPath(tamYol), contentType, dosyaAdi);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ÖĞRENCİ ARAMA
    // ═══════════════════════════════════════════════════════════════════════

    public async Task<IActionResult> OgrenciAra(string? arama, int? bolumId)
    {
        var sonuclar = await _service.OgrenciAraAsync(arama, bolumId);
        var bolumler = await _service.GetBolumSelectListAsync();

        ViewBag.Bolumler = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            bolumler, "Id", "DisplayText", bolumId);

        var vm = new OgrenciAraViewModel
        {
            Arama    = arama,
            BolumId  = bolumId,
            Sonuclar = sonuclar
        };
        return View(vm);
    }


    // ═══════════════════════════════════════════════════════════════════════
    // ÖĞRENCİ DETAY
    // ═══════════════════════════════════════════════════════════════════════
    public async Task<IActionResult> OgrenciDetay(int id)
    {
        var result = await _service.OgrenciDetayAsync(id);
        if (result == null || result.Data == null)
        {
            SetErrorMessage("Öğrenci bulunamadı.");
            return RedirectToAction(nameof(OgrenciAra));
        }
        return View(result.Data);
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