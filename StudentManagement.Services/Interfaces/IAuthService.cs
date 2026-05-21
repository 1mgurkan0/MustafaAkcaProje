using StudentManagement.Services.ViewModels.Auth;
using StudentManagement.Services.Helpers;
using StudentManagement.Services.ViewModels.Admin;

namespace StudentManagement.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<LoginSessionData>> LoginAsync(LoginViewModel model, string? ipAddress);
    Task<ServiceResult> RegisterAsync(RegisterViewModel model);
    Task<IEnumerable<BolumSelectViewModel>> GetBolumlerAsync();
}