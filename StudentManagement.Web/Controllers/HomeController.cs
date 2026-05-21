using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.Constants;
using StudentManagement.Core.Enums; // Eğer enumların buradaysa

namespace StudentManagement.Web.Controllers;

/// <summary>
/// Uygulama giriş trafiğini yöneten merkezi kontrolcü.
/// </summary>
public class HomeController : BaseController
{
    [HttpGet]
    public IActionResult Index()
    {
        // Session'dan veriyi çek
        var userRole = HttpContext.Session.GetString(AppConstants.Session.UserRole);

        // DEBUG İÇİN: Eğer yine patlarsa buraya bir breakpoint koyup userRole ne geliyor bak.
        if (string.IsNullOrEmpty(userRole))
        {
            return RedirectToAction("Login", "Auth");
        }

        // Senior tipi yönlendirme: Explicit (Açık) yönlendirme yapalım
        if (userRole == "Admin")
            return RedirectToRoute(new { controller = "Admin", action = "Index" });

        if (userRole == "Ogretmen")
            return RedirectToRoute(new { controller = "Ogretmen", action = "Index" });

        if (userRole == "Ogrenci")
            return RedirectToRoute(new { controller = "Ogrenci", action = "Dashboard" });

        // Hiçbiri değilse oturumu patlat
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Auth");
    }
}