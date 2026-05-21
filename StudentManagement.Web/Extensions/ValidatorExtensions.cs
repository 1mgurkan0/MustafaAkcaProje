using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Services.Validators.Admin;
using StudentManagement.Services.Validators.Auth;
using StudentManagement.Services.Validators.Panel;

namespace StudentManagement.Services.Extensions;

public static class ValidatorExtensions
{
    public static IServiceCollection AddUbysValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        // Auth
        services.AddValidatorsFromAssemblyContaining<LoginViewModelValidator>();

        // Admin
        services.AddValidatorsFromAssemblyContaining<BolumOlusturValidator>();

        // Panel
        services.AddValidatorsFromAssemblyContaining<NotGirViewModelValidator>();

        return services;
    }
}
