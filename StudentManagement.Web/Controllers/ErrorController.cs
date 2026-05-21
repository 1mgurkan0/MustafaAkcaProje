using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.Web.Controllers;

public class ErrorController : Controller
{
    [HttpGet]
    public IActionResult NotFound()
    {
        ViewData["StatusCode"] = "404";
        ViewData["Message"]    = "Aradığınız sayfa bulunamadı.";
        return View("Error");
    }

    [HttpGet]
    public IActionResult BadRequest()
    {
        ViewData["StatusCode"] = "400";
        ViewData["Message"]    = "Geçersiz istek.";
        return View("Error");
    }

    [HttpGet]
    public IActionResult ServerError()
    {
        ViewData["StatusCode"] = "500";
        ViewData["Message"]    = "Sunucu hatası oluştu.";
        return View("Error");
    }
}
