using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StudentManagement.Core.Constants;

namespace StudentManagement.Web.Filters;

public class SessionAuthFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.Session.GetInt32(AppConstants.Session.UserId);
        if (userId is null)
            context.Result = new RedirectToActionResult("Login", "Auth", null);
    }
}
