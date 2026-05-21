using StudentManagement.Core.Constants;
using StudentManagement.Core.Enums;

namespace StudentManagement.Web.Middlewares;

public class SessionValidationMiddleware
{
    private readonly RequestDelegate _next;

    // Oturum gerektirmeyen path'ler
    private static readonly string[] PublicPaths =
    {
        "/Auth/Login", "/Auth/Register", "/Auth/AccessDenied", "/Error"
    };

    public SessionValidationMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var path     = context.Request.Path.Value ?? string.Empty;
        var isPublic = PublicPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase));

        if (!isPublic)
        {
            var userId = context.Session.GetInt32(AppConstants.Session.UserId);

            // Oturum yoksa → login'e yönlendir
            if (userId is null)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }

            // Kök path ("/") veya eski Home/Index → role göre dashboard'a yönlendir
            if (path == "/" || path.Equals("/Home/Index", StringComparison.OrdinalIgnoreCase))
            {
                var roleStr = context.Session.GetString(AppConstants.Session.UserRole);
                if (Enum.TryParse<KullaniciRol>(roleStr, out var role))
                {
                    var redirectUrl = role switch
                    {
                        KullaniciRol.Admin         => "/Admin/Dashboard",
                        KullaniciRol.Ogretmen      => "/OgretmenPanel/Dashboard",
                        KullaniciRol.OgrenciIsleri => "/OgrenciIsleriPanel/Dashboard",
                        KullaniciRol.Ogrenci       => "/OgrenciPanel/Dashboard",
                        _                          => "/Auth/Login"
                    };
                    context.Response.Redirect(redirectUrl);
                    return;
                }
            }
        }

        await _next(context);
    }
}
