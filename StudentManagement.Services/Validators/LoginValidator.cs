using FluentValidation;
using StudentManagement.Services.ViewModels.Auth;

namespace StudentManagement.Services.Validators;

public class LoginValidator : AbstractValidator<LoginViewModel>
{
    public LoginValidator()
    {
        RuleFor(x => x.KullaniciAdi).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Sifre).NotEmpty();
    }
}
