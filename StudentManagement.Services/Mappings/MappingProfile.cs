using AutoMapper;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;
using StudentManagement.Services.ViewModels.Admin;
using StudentManagement.Services.ViewModels.Auth;
using StudentManagement.Services.ViewModels.OgrenciIsleri;
using StudentManagement.Services.ViewModels.Common;
using StudentManagement.Services.ViewModels.Ogrenci;
using StudentManagement.Services.ViewModels.Ogretmen;

namespace StudentManagement.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ═══════════════════════════════════════════════
        // BÖLÜM
        // ═══════════════════════════════════════════════
        CreateMap<Bolum, BolumListViewModel>();
        CreateMap<Bolum, BolumViewModel>();
        CreateMap<BolumOlusturViewModel, Bolum>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Bolum, BolumOlusturViewModel>();
        CreateMap<Bolum, BolumDuzenleViewModel>();
        CreateMap<BolumDuzenleViewModel, Bolum>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Bolum, BolumSelectViewModel>()
            .ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src =>
                $"{src.BolumKodu} - {src.BolumAdi}"));

        // ═══════════════════════════════════════════════
        // DÖNEM
        // ═══════════════════════════════════════════════
        CreateMap<Donem, DonemViewModel>()
            .ForMember(dest => dest.DonemTurAdi, opt => opt.MapFrom(src => src.DonemTur.ToString()))
            .ForMember(dest => dest.DersAtamaSayisi, opt => opt.MapFrom(src =>
                src.DersAtamalari != null ? src.DersAtamalari.Count : 0));

        CreateMap<DonemOlusturViewModel, Donem>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.AktifMi, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Donem, DonemOlusturViewModel>();
        CreateMap<Donem, DonemDuzenleViewModel>();
        CreateMap<DonemDuzenleViewModel, Donem>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Donem, DonemSelectViewModel>()
            .ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src =>
                $"{src.Yil} {src.DonemTur} {(src.AktifMi ? "(Aktif)" : "")}".Trim()));

        // ═══════════════════════════════════════════════
        // DERS (Katalog)
        // ═══════════════════════════════════════════════
        CreateMap<Ders, DersViewModel>()
            .ForMember(dest => dest.BolumAdi, opt => opt.MapFrom(src =>
                src.Bolum != null ? src.Bolum.BolumAdi : string.Empty))
            .ForMember(dest => dest.BolumKodu, opt => opt.MapFrom(src =>
                src.Bolum != null ? src.Bolum.BolumKodu : string.Empty));

        CreateMap<DersOlusturViewModel, Ders>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Ders, DersOlusturViewModel>();
        CreateMap<Ders, DersDuzenleViewModel>();
        CreateMap<DersDuzenleViewModel, Ders>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Ders, DersSelectViewModel>()
            .ForMember(dest => dest.DisplayText, opt => opt.MapFrom(src =>
                $"{src.DersKodu} - {src.DersAdi} ({src.Akts} AKTS)"));

        // ═══════════════════════════════════════════════
        // DERS ATAMA
        // ═══════════════════════════════════════════════
        CreateMap<DersAtama, DersAtamaViewModel>()
            .ForMember(dest => dest.DersAdi, opt => opt.MapFrom(src =>
                src.Ders != null ? src.Ders.DersAdi : string.Empty))
            .ForMember(dest => dest.DersKodu, opt => opt.MapFrom(src =>
                src.Ders != null ? src.Ders.DersKodu : string.Empty))
            .ForMember(dest => dest.Akts, opt => opt.MapFrom(src =>
                src.Ders != null ? src.Ders.Akts : 0))
            .ForMember(dest => dest.MaxKontenjan, opt => opt.MapFrom(src =>
                src.Ders != null ? src.Ders.MaxKontenjan : 0))
            .ForMember(dest => dest.OgretmenAdi, opt => opt.MapFrom(src =>
                src.Ogretmen != null ? src.Ogretmen.TamAd : string.Empty))
            .ForMember(dest => dest.OgretmenUnvanliAd, opt => opt.MapFrom(src =>
                src.Ogretmen != null ? src.Ogretmen.UnvanliAd : string.Empty))
            .ForMember(dest => dest.DonemAdi, opt => opt.MapFrom(src =>
                src.Donem != null ? $"{src.Donem.Yil} {src.Donem.DonemTur}" : string.Empty))
            .ForMember(dest => dest.BolumAdi, opt => opt.MapFrom(src =>
                src.Ders != null && src.Ders.Bolum != null ? src.Ders.Bolum.BolumAdi : string.Empty))
            .ForMember(dest => dest.GunAdi, opt => opt.MapFrom(src => src.Gun.ToString()))
            .ForMember(dest => dest.DolulukOrani, opt => opt.MapFrom(src =>
                src.Ders != null && src.Ders.MaxKontenjan > 0
                    ? (double)src.KayitliOgrenciSayisi / src.Ders.MaxKontenjan * 100
                    : 0));

        CreateMap<DersAtamaOlusturViewModel, DersAtama>()
            .ForMember(dest => dest.KayitliOgrenciSayisi, opt => opt.MapFrom(_ => 0))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<DersAtama, DersAtamaOlusturViewModel>();

        // ═══════════════════════════════════════════════
        // ÖĞRENCİ
        // ═══════════════════════════════════════════════
        CreateMap<Ogrenci, StudentManagement.Services.ViewModels.Admin.OgrenciViewModel>()
            .ForMember(dest => dest.TamAd, opt => opt.MapFrom(src =>
                src.Kullanici != null ? src.Kullanici.TamAd : string.Empty))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src =>
                src.Kullanici != null ? src.Kullanici.Email : string.Empty))
            .ForMember(dest => dest.BolumAdi, opt => opt.MapFrom(src =>
                src.Bolum != null ? src.Bolum.BolumAdi : string.Empty))
            .ForMember(dest => dest.BolumKodu, opt => opt.MapFrom(src =>
                src.Bolum != null ? src.Bolum.BolumKodu : string.Empty))
            .ForMember(dest => dest.OgrenciDurumAdi, opt => opt.MapFrom(src =>
                src.Durum.ToString()));

        CreateMap<Ogrenci, StudentManagement.Services.ViewModels.Admin.OgrenciDetayViewModel>()
            .ForMember(dest => dest.TamAd, opt => opt.MapFrom(src =>
                src.Kullanici != null ? src.Kullanici.TamAd : string.Empty))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src =>
                src.Kullanici != null ? src.Kullanici.Email : string.Empty))
            .ForMember(dest => dest.BolumAdi, opt => opt.MapFrom(src =>
                src.Bolum != null ? src.Bolum.BolumAdi : string.Empty))
            .ForMember(dest => dest.OgrenciDurumAdi, opt => opt.MapFrom(src =>
                src.Durum.ToString()));

        // ═══════════════════════════════════════════════
        // ÖĞRENCİ DERS (OgrenciDers)
        // ═══════════════════════════════════════════════
        CreateMap<OgrenciDers, OgrenciDersViewModel>()
            .ForMember(dest => dest.DersAdi, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.Ders != null
                    ? src.DersAtama.Ders.DersAdi : string.Empty))
            .ForMember(dest => dest.DersKodu, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.Ders != null
                    ? src.DersAtama.Ders.DersKodu : string.Empty))
            .ForMember(dest => dest.Akts, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.Ders != null
                    ? src.DersAtama.Ders.Akts : 0))
            .ForMember(dest => dest.OgretmenAdi, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.Ogretmen != null
                    ? src.DersAtama.Ogretmen.UnvanliAd : string.Empty))
            .ForMember(dest => dest.Gun, opt => opt.MapFrom(src =>
                src.DersAtama != null ? src.DersAtama.Gun.ToString() : string.Empty))
            .ForMember(dest => dest.Saat, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.BaslangicSaati.HasValue && src.DersAtama.BitisSaati.HasValue
                    ? $"{src.DersAtama.BaslangicSaati.Value:hh\\:mm} - {src.DersAtama.BitisSaati.Value:hh\\:mm}" : "-"))
            .ForMember(dest => dest.Derslik, opt => opt.MapFrom(src =>
                src.DersAtama != null ? src.DersAtama.Derslik : string.Empty))
            .ForMember(dest => dest.DurumAdi, opt => opt.MapFrom(src => src.Durum.ToString()))
            .ForMember(dest => dest.HarfNotuAdi, opt => opt.MapFrom(src =>
                src.HarfNotu.HasValue ? src.HarfNotu.Value.ToString() : "-"));

        CreateMap<OgrenciDers, TranskriptSatirViewModel>()
            .ForMember(dest => dest.DersKodu, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.Ders != null
                    ? src.DersAtama.Ders.DersKodu : string.Empty))
            .ForMember(dest => dest.DersAdi, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.Ders != null
                    ? src.DersAtama.Ders.DersAdi : string.Empty))
            .ForMember(dest => dest.Akts, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.Ders != null
                    ? src.DersAtama.Ders.Akts : 0))
            .ForMember(dest => dest.DonemAdi, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.Donem != null
                    ? $"{src.DersAtama.Donem.Yil} {src.DersAtama.Donem.DonemTur}" : string.Empty))
            .ForMember(dest => dest.HarfNotuAdi, opt => opt.MapFrom(src =>
                src.HarfNotu.HasValue ? src.HarfNotu.Value.ToString() : "-"));

        // ═══════════════════════════════════════════════
        // SINAV
        // ═══════════════════════════════════════════════
        CreateMap<Sinav, SinavViewModel>()
            .ForMember(dest => dest.DersAdi, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.Ders != null
                    ? src.DersAtama.Ders.DersAdi : string.Empty))
            .ForMember(dest => dest.DersKodu, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.Ders != null
                    ? src.DersAtama.Ders.DersKodu : string.Empty))
            .ForMember(dest => dest.SinavTurAdi, opt => opt.MapFrom(src => src.SinavTur.ToString()));

        CreateMap<SinavEkleViewModel, Sinav>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // ═══════════════════════════════════════════════
        // YOKLAMA
        // ═══════════════════════════════════════════════
        CreateMap<Yoklama, YoklamaViewModel>()
            .ForMember(dest => dest.DersAdi, opt => opt.MapFrom(src =>
                src.DersAtama != null && src.DersAtama.Ders != null
                    ? src.DersAtama.Ders.DersAdi : string.Empty))
            .ForMember(dest => dest.OgrenciYoklamalar, opt => opt.MapFrom(src =>
                src.OgrenciYoklamalar));

        CreateMap<OgrenciYoklama, OgrenciYoklamaViewModel>()
            .ForMember(dest => dest.OgrenciNo, opt => opt.MapFrom(src =>
                src.Ogrenci != null ? src.Ogrenci.OgrenciNo : string.Empty))
            .ForMember(dest => dest.OgrenciAdi, opt => opt.MapFrom(src =>
                src.Ogrenci != null && src.Ogrenci.Kullanici != null
                    ? src.Ogrenci.Kullanici.TamAd : string.Empty));

        // ═══════════════════════════════════════════════
        // DUYURU
        // ═══════════════════════════════════════════════
        CreateMap<Duyuru, StudentManagement.Services.ViewModels.Common.DuyuruOlusturViewModel>()
      .ForMember(dest => dest.HedefAdi, opt => opt.MapFrom(src => src.Hedef.ToString()))
      .ForMember(dest => dest.YayinlayanAdi, opt => opt.MapFrom(src => src.Yayinlayan != null ? src.Yayinlayan.TamAd : string.Empty))
      .ForMember(dest => dest.DersAdi, opt => opt.MapFrom(src => src.HedefDersAtama != null && src.HedefDersAtama.Ders != null ? src.HedefDersAtama.Ders.DersAdi : string.Empty));

        // Ogretmen içindeki CreateViewModel ile Map'le
        CreateMap<StudentManagement.Services.ViewModels.Ogretmen.DuyuruOlusturViewModel, Duyuru>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // ═══════════════════════════════════════════════
        // BELGE TALEBİ
        // ═══════════════════════════════════════════════
        CreateMap<BelgeTalebi, StudentManagement.Services.ViewModels.OgrenciIsleri.BelgeTalebiViewModel>()
            .ForMember(dest => dest.BelgeTurAdi, opt => opt.MapFrom(src => src.BelgeTur.ToString()))
            .ForMember(dest => dest.BelgeDurumAdi, opt => opt.MapFrom(src => src.Durum.ToString()))
            .ForMember(dest => dest.OgrenciAdi, opt => opt.MapFrom(src =>
                src.Ogrenci != null && src.Ogrenci.Kullanici != null
                    ? src.Ogrenci.Kullanici.TamAd : string.Empty))
            .ForMember(dest => dest.OgrenciNo, opt => opt.MapFrom(src =>
                src.Ogrenci != null ? src.Ogrenci.OgrenciNo : string.Empty))
            .ForMember(dest => dest.IslemYapanAdi, opt => opt.MapFrom(src =>
                src.IslemYapan != null ? src.IslemYapan.TamAd : string.Empty));

        CreateMap<BelgeTalebiOlusturViewModel, BelgeTalebi>()
            .ForMember(dest => dest.Durum, opt => opt.MapFrom(_ => BelgeDurum.Beklemede))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // ═══════════════════════════════════════════════
        // AUDIT LOG
        // ═══════════════════════════════════════════════
        CreateMap<AuditLog, AuditLogViewModel>()
            .ForMember(dest => dest.ActionAdi, opt => opt.MapFrom(src => src.Action.ToString()))
            .ForMember(dest => dest.KullaniciAdi, opt => opt.MapFrom(src =>
                src.Kullanici != null ? src.Kullanici.TamAd : src.KullaniciId.ToString()));
    }
}