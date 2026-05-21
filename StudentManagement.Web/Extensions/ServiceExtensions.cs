using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data.Context;
using StudentManagement.Data.Repositories;
using StudentManagement.Data.UnitOfWork;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.Services;
using StudentManagement.Services.Service;
using StudentManagement.Services.Helpers;
using StudentManagement.Services.Implementations;
namespace StudentManagement.Services.Extensions;

public static class ServiceExtensions
{
    // ─── Data Katmanı ────────────────────────────────────────────────────────
    public static IServiceCollection AddDataServices(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(opts =>
            opts.UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly("StudentManagement.Data")));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // AuditLog — direkt DbContext inject eder
        services.AddScoped<IAuditLogWriter, AuditLogWriter>();

        return services;
    }

    // ─── Servis Katmanı ──────────────────────────────────────────────────────
    public static IServiceCollection AddUbysServices(this IServiceCollection services)
    {
        // Auth
        services.AddScoped<IAuthService, AuthService>();

        // Panel servisleri
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IOgretmenPanelService, OgretmenPanelService>();
        services.AddScoped<IOgrenciPanelService, OgrenciPanelService>();
        services.AddScoped<IOgrenciIsleriService, OgrenciIsleriService>();

        // Audit Log
        services.AddScoped<IAuditLogService, AuditLogService>();

        return services;
    }
}