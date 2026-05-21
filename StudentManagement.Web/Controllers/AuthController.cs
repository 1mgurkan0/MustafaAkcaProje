using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.Constants;
using StudentManagement.Core.Enums;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.ViewModels.Auth;

namespace StudentManagement.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger      = logger;
    }

    // ─── GET Login ────────────────────────────────────────────────────────────
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    // ─── POST Login ───────────────────────────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        Console.WriteLine("\n\n==========================================");
        Console.WriteLine($"[DEBUG] LOGIN POST İSTEĞİ GELDİ! KULLANICI: {model.KullaniciAdi}");
        Console.WriteLine("==========================================\n\n");

        TempData["DebugMessage"] = $"POST İsteği Ulaştı: {model.KullaniciAdi}. ";

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["DebugMessage"] += "HATA: Form eksik veya geçersiz! -> " + string.Join(", ", errors);
            ModelState.AddModelError(string.Empty, "Lütfen alanları eksiksiz doldurun: " + string.Join(", ", errors));
            return View(model);
        }

        var result = await _authService.LoginAsync(model, HttpContext.Connection.RemoteIpAddress?.ToString());

        if (!result.IsSuccess)
        {
            TempData["DebugMessage"] += "GİRİŞ BAŞARISIZ: " + result.Message;
            ModelState.AddModelError(string.Empty, "Giriş Başarısız: " + result.Message);
            _logger.LogWarning("Başarısız giriş: {Username} - Neden: {Reason}", model.KullaniciAdi, result.Message);
            return View(model);
        }

        TempData["DebugMessage"] += "GİRİŞ BAŞARILI! Yönlendiriliyor...";

        // Session'a yaz
        HttpContext.Session.SetInt32(AppConstants.Session.UserId,    result.Data!.UserId);
        HttpContext.Session.SetString(AppConstants.Session.Username,  result.Data.Username);
        HttpContext.Session.SetString(AppConstants.Session.UserRole,  result.Data.Role);
        HttpContext.Session.SetString(AppConstants.Session.FullName,  result.Data.FullName);

        if (result.Data.OgrenciId.HasValue)
            HttpContext.Session.SetInt32(AppConstants.Session.OgrenciId, result.Data.OgrenciId.Value);
        if (result.Data.BolumId.HasValue)
            HttpContext.Session.SetInt32(AppConstants.Session.BolumId, result.Data.BolumId.Value);

        _logger.LogInformation("Giriş başarılı: {Username} [{Role}]",
            result.Data.Username, result.Data.Role);

        // ReturnUrl güvenlik kontrolü
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return result.Data.Role switch
        {
            nameof(KullaniciRol.Admin)         => Redirect(AppConstants.Routes.AdminBase),
            nameof(KullaniciRol.Ogretmen)      => Redirect(AppConstants.Routes.OgretmenBase),
            nameof(KullaniciRol.Ogrenci)       => Redirect(AppConstants.Routes.OgrenciBase),
            nameof(KullaniciRol.OgrenciIsleri) => Redirect(AppConstants.Routes.OgrenciIsleriBase),
            _                                  => Redirect(AppConstants.Routes.Login)
        };
    }

    // ─── GET Register ─────────────────────────────────────────────────────────
    [HttpGet]
    public async Task<IActionResult> Register()
    {
        var bolumler = await _authService.GetBolumlerAsync();
        ViewBag.Bolumler = bolumler
            .Select(b => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = b.Id.ToString(),
                Text  = b.DisplayText
            })
            .ToList();

        return View(new RegisterViewModel());
    }

    // ─── POST Register ────────────────────────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await RepopulateBolumler();
            return View(model);
        }

        var result = await _authService.RegisterAsync(model);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            await RepopulateBolumler();
            return View(model);
        }

        TempData[AppConstants.TempDataKeys.SuccessMessage] = "Kayıt başarılı! Giriş yapabilirsiniz.";
        return RedirectToAction(nameof(Login));
    }

    // ─── POST Logout ──────────────────────────────────────────────────────────
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        var username = HttpContext.Session.GetString(AppConstants.Session.Username);
        HttpContext.Session.Clear();
        _logger.LogInformation("Çıkış yapıldı: {Username}", username);
        return RedirectToAction(nameof(Login));
    }

    // ─── Private ─────────────────────────────────────────────────────────────
    private async Task RepopulateBolumler()
    {
        var bolumler = await _authService.GetBolumlerAsync();
        ViewBag.Bolumler = bolumler
            .Select(b => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = b.Id.ToString(),
                Text  = b.DisplayText
            })
            .ToList();
    }
}
