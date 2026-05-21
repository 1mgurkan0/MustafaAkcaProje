using FluentValidation;
using StudentManagement.Core.Constants;
using StudentManagement.Services.ViewModels.Auth;

namespace StudentManagement.Services.Validators;

public class RegisterValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterValidator()
    {
        RuleFor(x => x.KullaniciAdi).NotEmpty().MinimumLength(3).MaximumLength(50)
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Sadece harf, rakam ve alt çizgi kullanılabilir.");
        RuleFor(x => x.Ad).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Soyad).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Sifre).NotEmpty().MinimumLength(AppConstants.PasswordMinLength)
            .Matches("[A-Z]").WithMessage("En az bir büyük harf içermelidir.")
            .Matches("[a-z]").WithMessage("En az bir küçük harf içermelidir.")
            .Matches("[0-9]").WithMessage("En az bir rakam içermelidir.")
            .Matches("[^a-zA-Z0-9]").WithMessage("En az bir özel karakter içermelidir.");
        RuleFor(x => x.SifreTekrar).Equal(x => x.Sifre).WithMessage("Şifreler eşleşmiyor.");
    }
}
