using FluentValidation;
using StudentManagement.Services.ViewModels.Auth;

namespace StudentManagement.Services.Validators.Auth;

public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginViewModelValidator()
    {
        RuleFor(x => x.KullaniciAdi)
            .NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz.")
            .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir.");

        RuleFor(x => x.Sifre)
            .NotEmpty().WithMessage("Şifre boş bırakılamaz.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir.");
    }
}

public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterViewModelValidator()
    {
        RuleFor(x => x.KullaniciAdi)
            .NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz.")
            .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir.")
            .Matches(@"^[a-zA-Z0-9._-]+$").WithMessage("Kullanıcı adı yalnızca harf, rakam, nokta, tire ve alt çizgi içerebilir.");

        RuleFor(x => x.Ad)
            .NotEmpty().WithMessage("Ad boş bırakılamaz.")
            .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("Ad yalnızca harf içerebilir.");

        RuleFor(x => x.Soyad)
            .NotEmpty().WithMessage("Soyad boş bırakılamaz.")
            .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$").WithMessage("Soyad yalnızca harf içerebilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta boş bırakılamaz.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
            .MaximumLength(100).WithMessage("E-posta en fazla 100 karakter olabilir.");

        RuleFor(x => x.Sifre)
            .NotEmpty().WithMessage("Şifre boş bırakılamaz.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olabilir.")
            .Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
            .Matches(@"[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
            .Matches(@"[0-9]").WithMessage("Şifre en az bir rakam içermelidir.");

        RuleFor(x => x.SifreTekrar)
            .NotEmpty().WithMessage("Şifre tekrarı boş bırakılamaz.")
            .Equal(x => x.Sifre).WithMessage("Şifreler eşleşmiyor.");

        RuleFor(x => x.BolumId)
            .GreaterThan(0).WithMessage("Bölüm seçimi zorunludur.");
    }
}
