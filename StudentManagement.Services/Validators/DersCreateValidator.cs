using FluentValidation;
using StudentManagement.Services.ViewModels.Ders;

namespace StudentManagement.Services.Validators;

public class DersCreateValidator : AbstractValidator<DersCreateViewModel>
{
    public DersCreateValidator()
    {
        RuleFor(x => x.DersKodu).NotEmpty().MaximumLength(20)
            .Matches("^[A-Z0-9]+$").WithMessage("Ders kodu büyük harf ve rakamdan oluşmalıdır.");
        RuleFor(x => x.DersAdi).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Kredi).InclusiveBetween(1, 10);
        RuleFor(x => x.Akts).InclusiveBetween(1, 30);
        RuleFor(x => x.SaatlikDersSayisi).InclusiveBetween(1, 40);
        RuleFor(x => x.OgretmenAdi).MaximumLength(150);
        RuleFor(x => x.Aciklama).MaximumLength(1000);
        RuleFor(x => x.Donem).MaximumLength(50);
    }
}
