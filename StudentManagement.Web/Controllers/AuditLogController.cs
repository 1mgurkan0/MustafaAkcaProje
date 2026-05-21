using Microsoft.AspNetCore.Mvc;
using StudentManagement.Core.Enums;
using StudentManagement.Services.Interfaces;
using StudentManagement.Web.Filters;
using StudentManagement.Services.Helpers;

namespace StudentManagement.Web.Controllers;

public class AuditLogController : BaseController
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogController(IAuditLogService auditLogService)
        => _auditLogService = auditLogService;

    [HttpGet]
    [RoleAuthFilter(KullaniciRol.Admin)]
    public IActionResult Index() => View();

    [HttpPost]
    [RoleAuthFilter(KullaniciRol.Admin)]
    public async Task<IActionResult> GetDataTable([FromBody] DataTableRequest request)
    {
        var result = await _auditLogService.GetDataTableAsync(request);
        return Json(result);
    }
}
