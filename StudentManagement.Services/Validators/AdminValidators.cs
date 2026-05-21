using FluentValidation;
using StudentManagement.Services.ViewModels.Admin;

namespace StudentManagement.Services.Validators.Admin;

// ─── BÖLÜM ──────────────────────────────────────────────────────────────────
public class BolumOlusturValidator : AbstractValidator<BolumOlusturViewModel>
{
    public BolumOlusturValidator()
    {
        RuleFor(x => x.BolumKodu)
            .NotEmpty().WithMessage("Bölüm kodu zorunludur.")
            .MaximumLength(10).WithMessage("Bölüm kodu en fazla 10 karakter olabilir.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Bölüm kodu yalnızca büyük harf ve rakam içerebilir.");

        RuleFor(x => x.BolumAdi)
            .NotEmpty().WithMessage("Bölüm adı zorunludur.")
            .MaximumLength(100).WithMessage("Bölüm adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.MinMezuniyetAkts)
            .InclusiveBetween(60, 300).WithMessage("Minimum mezuniyet AKTS 60-300 arasında olmalıdır.");
    }
}

public class BolumDuzenleValidator : AbstractValidator<BolumDuzenleViewModel>
{
    public BolumDuzenleValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.BolumKodu)
            .NotEmpty().WithMessage("Bölüm kodu zorunludur.")
            .MaximumLength(10).WithMessage("Bölüm kodu en fazla 10 karakter olabilir.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Bölüm kodu yalnızca büyük harf ve rakam içerebilir.");

        RuleFor(x => x.BolumAdi)
            .NotEmpty().WithMessage("Bölüm adı zorunludur.")
            .MaximumLength(100).WithMessage("Bölüm adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.MinMezuniyetAkts)
            .InclusiveBetween(60, 300).WithMessage("Minimum mezuniyet AKTS 60-300 arasında olmalıdır.");
    }
}

// ─── DÖNEM ──────────────────────────────────────────────────────────────────
public class DonemOlusturValidator : AbstractValidator<DonemOlusturViewModel>
{
    public DonemOlusturValidator()
    {
        RuleFor(x => x.DonemKodu)
            .NotEmpty().WithMessage("Dönem kodu zorunludur.")
            .MaximumLength(20).WithMessage("Dönem kodu en fazla 20 karakter olabilir.")
            .Matches(@"^[A-Z0-9-_]+$").WithMessage("Dönem kodu geçersiz karakter içeriyor.");

        RuleFor(x => x.Yil)
            .InclusiveBetween(2000, 2100).WithMessage("Geçerli bir yıl giriniz.");

        RuleFor(x => x.DonemTur)
            .IsInEnum().WithMessage("Geçerli bir dönem türü seçiniz.");

        RuleFor(x => x.DersKayitBaslangic)
            .NotEmpty().WithMessage("Ders kayıt başlangıç tarihi zorunludur.");

        RuleFor(x => x.DersKayitBitis)
            .NotEmpty().WithMessage("Ders kayıt bitiş tarihi zorunludur.")
            .GreaterThan(x => x.DersKayitBaslangic)
            .WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");
    }
}

public class DonemDuzenleValidator : AbstractValidator<DonemDuzenleViewModel>
{
    public DonemDuzenleValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.DonemKodu)
            .NotEmpty().WithMessage("Dönem kodu zorunludur.")
            .MaximumLength(20).WithMessage("Dönem kodu en fazla 20 karakter olabilir.");

        RuleFor(x => x.Yil)
            .InclusiveBetween(2000, 2100).WithMessage("Geçerli bir yıl giriniz.");

        RuleFor(x => x.DersKayitBitis)
            .GreaterThan(x => x.DersKayitBaslangic)
            .WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");
    }
}

// ─── DERS (Katalog) ──────────────────────────────────────────────────────────
public class DersOlusturValidator : AbstractValidator<DersOlusturViewModel>
{
    public DersOlusturValidator()
    {
        RuleFor(x => x.DersKodu)
            .NotEmpty().WithMessage("Ders kodu zorunludur.")
            .MaximumLength(15).WithMessage("Ders kodu en fazla 15 karakter olabilir.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Ders kodu yalnızca büyük harf ve rakam içerebilir.");

        RuleFor(x => x.DersAdi)
            .NotEmpty().WithMessage("Ders adı zorunludur.")
            .MaximumLength(150).WithMessage("Ders adı en fazla 150 karakter olabilir.");

        RuleFor(x => x.Akts)
            .InclusiveBetween(1, 8).WithMessage("AKTS değeri 1-8 arasında olmalıdır.");

        RuleFor(x => x.MaxKontenjan)
            .InclusiveBetween(1, 500).WithMessage("Kontenjan 1-500 arasında olmalıdır.");

        RuleFor(x => x.BolumId)
            .GreaterThan(0).WithMessage("Bölüm seçimi zorunludur.");
    }
}

public class DersDuzenleValidator : AbstractValidator<DersDuzenleViewModel>
{
    public DersDuzenleValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.DersAdi)
            .NotEmpty().WithMessage("Ders adı zorunludur.")
            .MaximumLength(150).WithMessage("Ders adı en fazla 150 karakter olabilir.");

        RuleFor(x => x.Akts)
            .InclusiveBetween(1, 8).WithMessage("AKTS değeri 1-8 arasında olmalıdır.");

        RuleFor(x => x.MaxKontenjan)
            .InclusiveBetween(1, 500).WithMessage("Kontenjan 1-500 arasında olmalıdır.");
    }
}

// ─── DERS ATAMA ──────────────────────────────────────────────────────────────
public class DersAtamaOlusturValidator : AbstractValidator<DersAtamaOlusturViewModel>
{
    public DersAtamaOlusturValidator()
    {
        RuleFor(x => x.DersId)
            .GreaterThan(0).WithMessage("Ders seçimi zorunludur.");

        RuleFor(x => x.DonemId)
            .GreaterThan(0).WithMessage("Dönem seçimi zorunludur.");

        RuleFor(x => x.OgretmenId)
            .GreaterThan(0).WithMessage("Öğretmen seçimi zorunludur.");

        RuleFor(x => x.Gun)
            .IsInEnum().WithMessage("Geçerli bir gün seçiniz.");

        RuleFor(x => x.BaslangicSaati)
            .NotEmpty().WithMessage("Başlangıç saati zorunludur.");

        RuleFor(x => x.BitisSaati)
            .NotEmpty().WithMessage("Bitiş saati zorunludur.")
            .GreaterThan(x => x.BaslangicSaati).WithMessage("Bitiş saati başlangıç saatinden sonra olmalıdır.");

        RuleFor(x => x.Derslik)
            .NotEmpty().WithMessage("Derslik bilgisi zorunludur.")
            .MaximumLength(20).WithMessage("Derslik en fazla 20 karakter olabilir.");
    }
}
