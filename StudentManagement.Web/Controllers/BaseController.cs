using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentManagement.Core.Constants;
using StudentManagement.Core.Enums;
using StudentManagement.Web.Filters;

namespace StudentManagement.Web.Controllers;

[TypeFilter(typeof(SessionAuthFilter))]
public abstract class BaseController : Controller
{
    protected int CurrentUserId =>
        HttpContext.Session.GetInt32(AppConstants.Session.UserId) ?? 0;
    protected int CurrentOgrenciId =>
        HttpContext.Session.GetInt32(AppConstants.Session.OgrenciId) ?? 0;
    protected int CurrentBolumId =>
        HttpContext.Session.GetInt32(AppConstants.Session.BolumId) ?? 0;
    protected string CurrentUsername =>
        HttpContext.Session.GetString(AppConstants.Session.Username) ?? string.Empty;
    protected string CurrentFullName =>
        HttpContext.Session.GetString(AppConstants.Session.FullName) ?? string.Empty;
    protected KullaniciRol CurrentRole
    {
        get
        {
            var roleStr = HttpContext.Session.GetString(AppConstants.Session.UserRole);
            return Enum.TryParse<KullaniciRol>(roleStr, out var role) ? role : KullaniciRol.Ogrenci;
        }
    }
    protected string IpAddress =>
        HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    protected void SetSuccessMessage(string message) => TempData["SuccessMessage"] = message;
    protected void SetErrorMessage(string message)   => TempData["ErrorMessage"]   = message;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.CurrentUsername = CurrentUsername;
        ViewBag.CurrentFullName = CurrentFullName;
        ViewBag.CurrentRole     = CurrentRole.ToString();
        base.OnActionExecuting(context);
    }
}
