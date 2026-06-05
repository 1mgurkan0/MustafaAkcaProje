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

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        var role = HttpContext.Session.GetString(AppConstants.Session.UserRole);
        if (!string.IsNullOrEmpty(role))
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            ModelState.AddModelError(string.Empty, "Lütfen alanları eksiksiz doldurun: " + string.Join(", ", errors));
            return View(model);
        }

        var result = await _authService.LoginAsync(model, HttpContext.Connection.RemoteIpAddress?.ToString());

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, "Giriş Başarısız: " + result.Message);
            _logger.LogWarning("Başarısız giriş: {Username} - Neden: {Reason}", model.KullaniciAdi, result.Message);
            return View(model);
        }

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


    [HttpGet]
    public async Task<IActionResult> Register()
    {
        var role = HttpContext.Session.GetString(AppConstants.Session.UserRole);
        if (!string.IsNullOrEmpty(role))
            return RedirectToAction("Index", "Home");

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        var username = HttpContext.Session.GetString(AppConstants.Session.Username);
        HttpContext.Session.Clear();
        _logger.LogInformation("Çıkış yapıldı: {Username}", username);
        return RedirectToAction(nameof(Login));
    }

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
