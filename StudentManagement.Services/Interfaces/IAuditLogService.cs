using StudentManagement.Services.ViewModels.Admin;
using StudentManagement.Services.Helpers;

namespace StudentManagement.Services.Interfaces;

public interface IAuditLogService
{
    Task<DataTableResponse<AuditLogViewModel>> GetDataTableAsync(DataTableRequest request);
    Task<IEnumerable<AuditLogViewModel>> GetByKullaniciAsync(int kullaniciId, int count = 20);
}
