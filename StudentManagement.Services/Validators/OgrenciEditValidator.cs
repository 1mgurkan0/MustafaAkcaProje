using FluentValidation;
using StudentManagement.Services.ViewModels.Ogrenci;

namespace StudentManagement.Services.Validators;

public class OgrenciEditValidator : AbstractValidator<OgrenciEditViewModel>
{
    public OgrenciEditValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.OgrenciNo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Bolum).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Sinif).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Ad).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Soyad).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.DogumTarihi).LessThan(DateTime.Today.AddYears(-15))
            .WithMessage("Öğrenci en az 15 yaşında olmalıdır.");
    }
}
