using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.Constants;
using StudentManagement.Core.Enums;

namespace StudentManagement.Web.Controllers;

/// <summary>
/// Uygulama giriş trafiğini yöneten merkezi kontrolcü.
/// Giriş yapmış kullanıcıyı rolüne göre doğru panele yönlendirir.
/// </summary>
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var userRole = HttpContext.Session.GetString(AppConstants.Session.UserRole);

        if (string.IsNullOrEmpty(userRole))
            return RedirectToAction("Login", "Auth");

        return userRole switch
        {
            nameof(KullaniciRol.Admin)         => Redirect(AppConstants.Routes.AdminBase),
            nameof(KullaniciRol.Ogretmen)      => Redirect(AppConstants.Routes.OgretmenBase),
            nameof(KullaniciRol.Ogrenci)       => Redirect(AppConstants.Routes.OgrenciBase),
            nameof(KullaniciRol.OgrenciIsleri) => Redirect(AppConstants.Routes.OgrenciIsleriBase),
            _                                  => RedirectToAction("Login", "Auth")
        };
    }
}