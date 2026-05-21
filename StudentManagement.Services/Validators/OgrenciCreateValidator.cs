using FluentValidation;
using StudentManagement.Core.Constants;
using StudentManagement.Services.ViewModels.Ogrenci;

namespace StudentManagement.Services.Validators;

public class OgrenciCreateValidator : AbstractValidator<OgrenciCreateViewModel>
{
    public OgrenciCreateValidator()
    {
        RuleFor(x => x.OgrenciNo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Bolum).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Sinif).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DogumTarihi).LessThan(DateTime.Today.AddYears(-15))
            .WithMessage("Öğrenci en az 15 yaşında olmalıdır.");
        RuleFor(x => x.KullaniciAdi).NotEmpty().MinimumLength(3).MaximumLength(50)
            .Matches("^[a-zA-Z0-9_]+$");
        RuleFor(x => x.Ad).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Soyad).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Sifre).NotEmpty().MinimumLength(AppConstants.PasswordMinLength)
            .Matches("[A-Z]").Matches("[a-z]").Matches("[0-9]").Matches("[^a-zA-Z0-9]");
    }
}
