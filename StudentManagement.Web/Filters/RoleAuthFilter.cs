using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentManagement.Core.Constants;
using StudentManagement.Core.Enums;

namespace StudentManagement.Web.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class RoleAuthFilter : Attribute, IAuthorizationFilter
{
    private readonly KullaniciRol[] _roles;

    public RoleAuthFilter(params KullaniciRol[] roles) => _roles = roles;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var session = context.HttpContext.Session;
        var userId  = session.GetInt32(AppConstants.Session.UserId);

        if (userId is null)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }

        if (_roles.Length == 0) return;

        var roleStr = session.GetString(AppConstants.Session.UserRole);

        if (string.IsNullOrEmpty(roleStr) ||
            !Enum.TryParse<KullaniciRol>(roleStr, out var role) ||
            !_roles.Contains(role))
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
        }
    }
}
