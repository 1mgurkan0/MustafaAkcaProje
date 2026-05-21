using FluentValidation;
using StudentManagement.Services.ViewModels.OgrenciIsleri;
using StudentManagement.Services.ViewModels.Ogretmen;

namespace StudentManagement.Services.Validators.Panel;

// ═══════════════════════════════════════════════════════
// ÖĞRETMEN PANEL
// ═══════════════════════════════════════════════════════

public class NotGirViewModelValidator : AbstractValidator<NotGirViewModel>
{
    public NotGirViewModelValidator()
    {
        RuleFor(x => x.DersAtamaId)
            .GreaterThan(0).WithMessage("Geçersiz ders ataması.");

        RuleForEach(x => x.Notlar).SetValidator(new OgrenciNotSatirValidator());
    }
}

public class OgrenciNotSatirValidator : AbstractValidator<OgrenciNotSatirViewModel>
{
    public OgrenciNotSatirValidator()
    {
        RuleFor(x => x.OgrenciDersId)
            .GreaterThan(0).WithMessage("Geçersiz öğrenci kaydı.");

        RuleFor(x => x.VizeNotu)
            .InclusiveBetween(0, 100).WithMessage("Vize notu 0-100 arasında olmalıdır.")
            .When(x => x.VizeNotu.HasValue);

        RuleFor(x => x.FinalNotu)
            .InclusiveBetween(0, 100).WithMessage("Final notu 0-100 arasında olmalıdır.")
            .When(x => x.FinalNotu.HasValue);

        RuleFor(x => x.ButunlemeNotu)
            .InclusiveBetween(0, 100).WithMessage("Bütünleme notu 0-100 arasında olmalıdır.")
            .When(x => x.ButunlemeNotu.HasValue);
    }
}

public class SinavEkleViewModelValidator : AbstractValidator<SinavEkleViewModel>
{
    public SinavEkleViewModelValidator()
    {
        RuleFor(x => x.DersAtamaId)
            .GreaterThan(0).WithMessage("Geçersiz ders ataması.");

        RuleFor(x => x.SinavTur)
            .IsInEnum().WithMessage("Geçerli bir sınav türü seçiniz.");

        RuleFor(x => x.SinavTarihi)
            .NotEmpty().WithMessage("Sınav tarihi zorunludur.")
            .GreaterThan(DateTime.Now.AddDays(-1))
            .WithMessage("Sınav tarihi geçmiş bir tarih olamaz.");

        RuleFor(x => x.Derslik)
            .MaximumLength(50).WithMessage("Sınav yeri en fazla 50 karakter olabilir.");

        RuleFor(x => x.Aciklama)
            .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
    }
}

public class YoklamaViewModelValidator : AbstractValidator<YoklamaGirViewModel>
{
    public YoklamaViewModelValidator()
    {
        RuleFor(x => x.DersAtamaId)
            .GreaterThan(0).WithMessage("Geçersiz ders ataması.");

        RuleFor(x => x.YoklamaTarihi)
            .NotEmpty().WithMessage("Yoklama tarihi zorunludur.")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Yoklama tarihi bugünden ileri bir tarih olamaz.");
    }
}

public class DuyuruOlusturViewModelValidator : AbstractValidator<DuyuruOlusturViewModel>
{
    public DuyuruOlusturViewModelValidator()
    {
        RuleFor(x => x.Baslik)
            .NotEmpty().WithMessage("Duyuru başlığı zorunludur.")
            .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

        RuleFor(x => x.Icerik)
            .NotEmpty().WithMessage("Duyuru içeriği zorunludur.")
            .MaximumLength(2000).WithMessage("İçerik en fazla 2000 karakter olabilir.");

        RuleFor(x => x.Hedef)
            .IsInEnum().WithMessage("Geçerli bir hedef kitle seçiniz.");

        RuleFor(x => x.HedefDersAtamaId)
            .GreaterThan(0).WithMessage("Ders seçimi zorunludur.")
            .When(x => x.Hedef == Core.Enums.DuyuruHedef.DersOzeli);
    }
}

// ═══════════════════════════════════════════════════════
// ÖĞRENCİ PANEL
// ═══════════════════════════════════════════════════════

public class DersKayitTalepValidator : AbstractValidator<DersKayitTalepViewModel>
{
    public DersKayitTalepValidator()
    {
        RuleFor(x => x.DersAtamaId)
            .GreaterThan(0).WithMessage("Geçerli bir ders seçiniz.");
    }
}

public class BelgeTalebiOlusturValidator : AbstractValidator<BelgeTalebiOlusturViewModel>
{
    public BelgeTalebiOlusturValidator()
    {
        RuleFor(x => x.BelgeTur)
            .IsInEnum().WithMessage("Geçerli bir belge türü seçiniz.");

        RuleFor(x => x.Aciklama)
            .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
    }
}

// ═══════════════════════════════════════════════════════
// ÖĞRENCİ İŞLERİ PANEL
// ═══════════════════════════════════════════════════════

public class TalepRedViewModelValidator : AbstractValidator<TalepRedViewModel>
{
    public TalepRedViewModelValidator()
    {
        RuleFor(x => x.OgrenciDersId)
            .GreaterThan(0).WithMessage("Geçersiz talep.");

        RuleFor(x => x.RedNedeni)
            .NotEmpty().WithMessage("Red nedeni zorunludur.")
            .MaximumLength(500).WithMessage("Red nedeni en fazla 500 karakter olabilir.");
    }
}

public class BelgeDurumGuncelleValidator : AbstractValidator<BelgeDurumGuncelleViewModel>
{
    public BelgeDurumGuncelleValidator()
    {
        RuleFor(x => x.BelgeTalebiId)
            .GreaterThan(0).WithMessage("Geçersiz belge talebi.");

        RuleFor(x => x.YeniBelgeDurum)
            .NotEmpty().WithMessage("Geçerli bir durum seçiniz.");
    }
}
