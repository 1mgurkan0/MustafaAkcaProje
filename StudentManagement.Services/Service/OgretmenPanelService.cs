using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentManagement.Core.Constants;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;
using StudentManagement.Data.Context;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.ViewModels.Admin;


namespace StudentManagement.Services.Services;

public class OgretmenPanelService : IOgretmenPanelService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IAuditLogWriter _audit;
    private readonly ILogger<OgretmenPanelService> _logger;

    public OgretmenPanelService(ApplicationDbContext db, IMapper mapper,
        IAuditLogWriter audit, ILogger<OgretmenPanelService> logger)
    {
        _db = db;
        _mapper = mapper;
        _audit = audit;
        _logger = logger;
    }

    // ─── DASHBOARD ────────────────────────────────────────────────────────────
    public async Task<OgretmenDashboardViewModel> GetDashboardAsync(int ogretmenId)
    {
        var ogretmen = await _db.Kullanicilar.FindAsync(ogretmenId);
        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);

        var aktifDersler = aktifDonem == null
            ? new List<DersAtamaOzetViewModel>()
            : await _db.DersAtamalar
                .Where(da => da.OgretmenId == ogretmenId && da.DonemId == aktifDonem.Id && da.IsActive)
                .Include(da => da.Ders)
                .Select(da => new DersAtamaOzetViewModel
                {
                    DersAtamaId = da.Id,
                    DersAdi = da.Ders!.DersAdi,
                    DersKodu = da.Ders.DersKodu,
                    Akts = da.Ders.Akts,
                    Gun = da.Gun.ToString(),
                    Saat = da.BaslangicSaati.HasValue && da.BitisSaati.HasValue 
                        ? $"{da.BaslangicSaati.Value:hh\\:mm} - {da.BitisSaati.Value:hh\\:mm}" 
                        : "-",
                    Derslik = da.Derslik,
                    KayitliOgrenciSayisi = da.KayitliOgrenciSayisi,
                    MaxKontenjan = da.Ders.MaxKontenjan,
                    DonemAdi = aktifDonem != null ? $"{aktifDonem.Yil} {aktifDonem.DonemTur}" : string.Empty
                })
                .ToListAsync();

        var yaklasanSinavlar = await _db.Sinavlar
            .Where(s => s.DersAtama!.OgretmenId == ogretmenId
                     && s.SinavTarihi >= DateTime.Now
                     && s.SinavTarihi <= DateTime.Now.AddDays(14)
                     && s.IsActive)
            .Include(s => s.DersAtama).ThenInclude(da => da!.Ders)
            .OrderBy(s => s.SinavTarihi)
            .Select(s => new SinavViewModel
            {
                Id = s.Id,
                DersAdi = s.DersAtama!.Ders!.DersAdi,
                DersKodu = s.DersAtama.Ders.DersKodu,
                SinavTurAdi = s.SinavTur.ToString(),
                SinavTarihi = s.SinavTarihi,
                Derslik = s.Derslik ?? string.Empty
            })
            .ToListAsync();

        return new OgretmenDashboardViewModel
        {
            UnvanliAd = ogretmen?.UnvanliAd ?? string.Empty,
            AktifDonemAdi = aktifDonem != null ? $"{aktifDonem.Yil} {aktifDonem.DonemTur}" : "—",
            AktifDersSayisi = aktifDersler.Count,
            ToplamOgrenciSayisi = aktifDersler.Sum(d => d.KayitliOgrenciSayisi),
            YaklasanSinavSayisi = yaklasanSinavlar.Count,
            DuyuruSayisi = await _db.Duyurular.CountAsync(d =>
                d.YayinlayanId == ogretmenId && d.IsActive),
            AktifDersler = aktifDersler,
            YaklasanSinavlar = yaklasanSinavlar
        };
    }

    // ─── DERSLERİM ────────────────────────────────────────────────────────────
    public async Task<IEnumerable<DersAtamaOzetViewModel>> GetDerslerimAsync(int ogretmenId)
    {
        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);
        if (aktifDonem == null) return Enumerable.Empty<DersAtamaOzetViewModel>();

        return await _db.DersAtamalar
            .Where(da => da.OgretmenId == ogretmenId && da.DonemId == aktifDonem.Id && da.IsActive)
            .Include(da => da.Ders)
            .Include(da => da.Donem)
            .OrderBy(da => da.Gun).ThenBy(da => da.BaslangicSaati)
            .Select(da => new DersAtamaOzetViewModel
            {
                DersAtamaId = da.Id,
                DersAdi = da.Ders!.DersAdi,
                DersKodu = da.Ders.DersKodu,
                Akts = da.Ders.Akts,
                Gun = da.Gun.ToString(),
                Saat = da.BaslangicSaati.HasValue && da.BitisSaati.HasValue 
                    ? $"{da.BaslangicSaati.Value:hh\\:mm} - {da.BitisSaati.Value:hh\\:mm}" 
                    : "-",
                Derslik = da.Derslik,
                KayitliOgrenciSayisi = da.KayitliOgrenciSayisi,
                MaxKontenjan = da.Ders.MaxKontenjan,
                DonemAdi = $"{da.Donem!.Yil} {da.Donem.DonemTur}"
            })
            .ToListAsync();
    }

    // ─── DERS DETAY ───────────────────────────────────────────────────────────
    public async Task<ServiceResult<DersDetayViewModel>> GetDersDetayAsync(int dersAtamaId, int ogretmenId)
    {
        var da = await _db.DersAtamalar
            .Include(x => x.Ders)
            .Include(x => x.Donem)
            .FirstOrDefaultAsync(x => x.Id == dersAtamaId && x.OgretmenId == ogretmenId && x.IsActive);

        if (da == null)
            return ServiceResult<DersDetayViewModel>.Fail(AppConstants.ErrorMessages.KayitBulunamadi);

        var ogrenciler = await _db.OgrenciDersler
            .Where(od => od.DersAtamaId == dersAtamaId &&
                         od.Durum == OgrenciDersDurum.Devam && od.IsActive)
            .Include(od => od.Ogrenci).ThenInclude(o => o!.Kullanici)
            .Select(od => new OgrenciNotSatirViewModel
            {
                OgrenciDersId = od.Id,
                OgrenciId = od.OgrenciId,
                OgrenciNo = od.Ogrenci!.OgrenciNo,
                OgrenciAdi = od.Ogrenci.Kullanici!.TamAd,
                VizeNotu = od.VizeNotu,
                FinalNotu = od.FinalNotu,
                ButunlemeNotu = od.ButunlemeNotu,
                GenelNot = od.GenelNot,
                HarfNotuAdi = od.HarfNotu.HasValue ? od.HarfNotu.Value.ToString() : "-",
                DevamYuzdesi = 0   // yoklama hesaplaması ayrı
            })
            .ToListAsync();

        // Devam yüzdelerini hesapla
        var toplamYoklama = await _db.Yoklamalar
            .CountAsync(y => y.DersAtamaId == dersAtamaId && y.IsActive);

        if (toplamYoklama > 0)
        {
            foreach (var ogr in ogrenciler)
            {
                var varSayisi = await _db.OgrenciYoklamalar
                    .CountAsync(oy => oy.OgrenciId == ogr.OgrenciId &&
                        oy.Yoklama!.DersAtamaId == dersAtamaId && oy.Geldi);
                ogr.DevamYuzdesi = (double)varSayisi / toplamYoklama * 100;
            }
        }

        var vm = new DersDetayViewModel
        {
            DersAtamaId = da.Id,
            DersAdi = da.Ders!.DersAdi,
            DersKodu = da.Ders.DersKodu,
            DonemAdi = $"{da.Donem!.Yil} {da.Donem.DonemTur}",
            Derslik = da.Derslik,
            KayitliOgrenciSayisi = da.KayitliOgrenciSayisi,
            YoklamaSayisi = toplamYoklama,
            SinavSayisi = await _db.Sinavlar.CountAsync(s => s.DersAtamaId == dersAtamaId && s.IsActive),
            DuyuruSayisi = await _db.Duyurular.CountAsync(d => d.HedefDersAtamaId == dersAtamaId && d.IsActive),
            Ogrenciler = ogrenciler
        };

        return ServiceResult<DersDetayViewModel>.Ok(vm);
    }

    // ─── NOT GİR ─────────────────────────────────────────────────────────────
    public async Task<ServiceResult<NotGirViewModel>> GetNotGirAsync(int dersAtamaId, int ogretmenId)
    {
        var da = await _db.DersAtamalar
            .Include(x => x.Ders)
            .Include(x => x.Donem)
            .FirstOrDefaultAsync(x => x.Id == dersAtamaId && x.OgretmenId == ogretmenId && x.IsActive);

        if (da == null)
            return ServiceResult<NotGirViewModel>.Fail(AppConstants.ErrorMessages.YetkiYok);

        var notlar = await _db.OgrenciDersler
            .Where(od => od.DersAtamaId == dersAtamaId &&
                         od.Durum == OgrenciDersDurum.Devam && od.IsActive)
            .Include(od => od.Ogrenci).ThenInclude(o => o!.Kullanici)
            .OrderBy(od => od.Ogrenci!.Kullanici!.Soyad)
            .Select(od => new OgrenciNotSatirViewModel
            {
                OgrenciDersId = od.Id,
                OgrenciId = od.OgrenciId,
                OgrenciNo = od.Ogrenci!.OgrenciNo,
                OgrenciAdi = od.Ogrenci.Kullanici!.TamAd,
                VizeNotu = od.VizeNotu,
                FinalNotu = od.FinalNotu,
                ButunlemeNotu = od.ButunlemeNotu,
                GenelNot = od.GenelNot,
                HarfNotuAdi = od.HarfNotu.HasValue ? od.HarfNotu.Value.ToString() : "-"
            })
            .ToListAsync();

        return ServiceResult<NotGirViewModel>.Ok(new NotGirViewModel
        {
            DersAtamaId = da.Id,
            DersAdi = da.Ders!.DersAdi,
            DersKodu = da.Ders.DersKodu,
            DonemAdi = $"{da.Donem!.Yil} {da.Donem.DonemTur}",
            Notlar = notlar
        });
    }

    public async Task<ServiceResult> NotKaydetAsync(NotGirViewModel model, int ogretmenId)
    {
        // Öğretmen bu derse atanmış mı?
        var daVarMi = await _db.DersAtamalar
            .AnyAsync(da => da.Id == model.DersAtamaId && da.OgretmenId == ogretmenId && da.IsActive);

        if (!daVarMi) return ServiceResult.Fail(AppConstants.ErrorMessages.YetkiYok);

        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            foreach (var satir in model.Notlar)
            {
                var od = await _db.OgrenciDersler.FindAsync(satir.OgrenciDersId);
                if (od == null) continue;

                od.VizeNotu = satir.VizeNotu;
                od.FinalNotu = satir.FinalNotu;
                od.ButunlemeNotu = satir.ButunlemeNotu;
                od.UpdatedAt = DateTime.UtcNow;
                od.UpdatedBy = ogretmenId;

                // Genel not hesapla
                var gecerliSonSinav = satir.ButunlemeNotu ?? satir.FinalNotu;
                if (satir.VizeNotu.HasValue && gecerliSonSinav.HasValue)
                {
                    od.GenelNot = satir.VizeNotu.Value * (decimal)AppConstants.Grading.VizeAgirlik
                                + gecerliSonSinav.Value * (decimal)AppConstants.Grading.FinalAgirlik;

                    od.HarfNotu = HesaplaHarfNotu((decimal)od.GenelNot.Value);
                }
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            await _audit.WriteAsync(ogretmenId, AuditAction.NotGir, "OgrenciDers",
                model.DersAtamaId, aciklama: $"Not girişi: {model.Notlar.Count} öğrenci");

            // GANO güncelle (arka planda)
            await GanoGuncelleAsync(model.DersAtamaId);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            _logger.LogError(ex, "NotKaydet hatası: DersAtamaId={Id}", model.DersAtamaId);
            return ServiceResult.Fail(AppConstants.ErrorMessages.GenelHata);
        }
    }

    // ─── YOKLAMA ─────────────────────────────────────────────────────────────
    public async Task<ServiceResult<YoklamaGirViewModel>> GetYoklamaGirAsync(int dersAtamaId, int ogretmenId)
    {
        var da = await _db.DersAtamalar
            .Include(x => x.Ders)
            .FirstOrDefaultAsync(x => x.Id == dersAtamaId && x.OgretmenId == ogretmenId && x.IsActive);

        if (da == null) return ServiceResult<YoklamaGirViewModel>.Fail(AppConstants.ErrorMessages.YetkiYok);

        var ogrenciler = await _db.OgrenciDersler
            .Where(od => od.DersAtamaId == dersAtamaId &&
                         od.Durum == OgrenciDersDurum.Devam && od.IsActive)
            .Include(od => od.Ogrenci).ThenInclude(o => o!.Kullanici)
            .OrderBy(od => od.Ogrenci!.Kullanici!.Soyad)
            .Select(od => new OgrenciYoklamaSatirViewModel
            {
                OgrenciId = od.OgrenciId,
                OgrenciNo = od.Ogrenci!.OgrenciNo,
                OgrenciAdi = od.Ogrenci.Kullanici!.TamAd,
                Geldi = true
            })
            .ToListAsync();

        return ServiceResult<YoklamaGirViewModel>.Ok(new YoklamaGirViewModel
        {
            DersAtamaId = dersAtamaId,
            DersAdi = da.Ders!.DersAdi,
            DonemAdi = string.Empty,
            YoklamaTarihi = DateTime.Today,
            OgrenciListesi = ogrenciler
        });
    }

    public async Task<ServiceResult> YoklamaKaydetAsync(YoklamaGirViewModel model, int ogretmenId)
    {
        var daVarMi = await _db.DersAtamalar
            .AnyAsync(da => da.Id == model.DersAtamaId && da.OgretmenId == ogretmenId && da.IsActive);

        if (!daVarMi) return ServiceResult.Fail(AppConstants.ErrorMessages.YetkiYok);

        // Aynı gün için yoklama var mı?
        var mevcutYoklama = await _db.Yoklamalar
            .FirstOrDefaultAsync(y => y.DersAtamaId == model.DersAtamaId
                               && y.YoklamaTarihi.Date == model.YoklamaTarihi.Date
                               && y.IsActive);

        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            int yoklamaId;
            if (mevcutYoklama == null)
            {
                var yoklama = new Yoklama
                {
                    DersAtamaId = model.DersAtamaId,
                    OgretmenId = ogretmenId,
                    YoklamaTarihi = model.YoklamaTarihi,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = ogretmenId
                };
                _db.Yoklamalar.Add(yoklama);
                await _db.SaveChangesAsync();
                yoklamaId = yoklama.Id;
            }
            else
            {
                // Eski yoklama kayıtlarını temizle
                var eskiler = _db.OgrenciYoklamalar
                    .Where(oy => oy.YoklamaId == mevcutYoklama.Id);
                _db.OgrenciYoklamalar.RemoveRange(eskiler);
                yoklamaId = mevcutYoklama.Id;
            }

            // Öğrenci yoklama kayıtları
            foreach (var ogr in model.OgrenciListesi)
            {
                _db.OgrenciYoklamalar.Add(new OgrenciYoklama
                {
                    YoklamaId = yoklamaId,
                    OgrenciId = ogr.OgrenciId,
                    Geldi = ogr.Geldi,
                    KayitTarihi = DateTime.UtcNow
                });
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            await _audit.WriteAsync(ogretmenId, AuditAction.YoklamaAl, "Yoklama", yoklamaId);
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            _logger.LogError(ex, "YoklamaKaydet hatası");
            return ServiceResult.Fail(AppConstants.ErrorMessages.GenelHata);
        }
    }

    // ─── SINAVLAR ────────────────────────────────────────────────────────────
    public async Task<SinavlarViewModel> GetSinavlarAsync(int ogretmenId)
    {
        var sinavlar = await _db.Sinavlar
            .Where(s => s.DersAtama!.OgretmenId == ogretmenId && s.IsActive)
            .Include(s => s.DersAtama).ThenInclude(da => da!.Ders)
            .OrderByDescending(s => s.SinavTarihi)
            .Select(s => new SinavViewModel
            {
                Id = s.Id,
                DersAdi = s.DersAtama!.Ders!.DersAdi,
                DersKodu = s.DersAtama.Ders.DersKodu,
                SinavTurAdi = s.SinavTur.ToString(),
                SinavTarihi = s.SinavTarihi,
                Derslik = s.Derslik ?? string.Empty,
                Aciklama = s.Aciklama ?? string.Empty
            })
            .ToListAsync();

        return new SinavlarViewModel { Sinavlar = sinavlar };
    }

    public async Task<ServiceResult> SinavEkleAsync(SinavEkleViewModel model, int ogretmenId)
    {
        // Öğretmen bu derse atanmış mı?
        var daVarMi = await _db.DersAtamalar
            .AnyAsync(da => da.Id == model.DersAtamaId && da.OgretmenId == ogretmenId && da.IsActive);

        if (!daVarMi) return ServiceResult.Fail(AppConstants.ErrorMessages.YetkiYok);

        var sinav = _mapper.Map<Sinav>(model);
        sinav.CreatedBy = ogretmenId;
        _db.Sinavlar.Add(sinav);
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(ogretmenId, AuditAction.Create, "Sinav", sinav.Id,
            yeniDeger: sinav.SinavTur.ToString());
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> SinavSilAsync(int id, int ogretmenId)
    {
        var sinav = await _db.Sinavlar
            .Include(s => s.DersAtama)
            .FirstOrDefaultAsync(s => s.Id == id && s.DersAtama!.OgretmenId == ogretmenId);

        if (sinav == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);

        sinav.IsActive = false;
        sinav.UpdatedAt = DateTime.UtcNow;
        sinav.UpdatedBy = ogretmenId;
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(ogretmenId, AuditAction.Delete, "Sinav", id);
        return ServiceResult.Ok();
    }

    // ─── DUYURU ──────────────────────────────────────────────────────────────
    public async Task<ServiceResult> DuyuruYayinlaAsync(DuyuruOlusturViewModel model, int ogretmenId)
    {
        var duyuru = _mapper.Map<Duyuru>(model);
        duyuru.YayinlayanId = ogretmenId;
        duyuru.CreatedBy = ogretmenId;
        _db.Duyurular.Add(duyuru);
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(ogretmenId, AuditAction.DuyuruYayinla, "Duyuru", duyuru.Id,
            yeniDeger: duyuru.Baslik);
        return ServiceResult.Ok();
    }

    public async Task<IEnumerable<DuyuruOlusturViewModel>> GetDuyurularAsync(int ogretmenId)
    {
        var ogretmenDersAtamaIdler = await _db.DersAtamalar
            .Where(da => da.OgretmenId == ogretmenId && da.IsActive)
            .Select(da => da.Id)
            .ToListAsync();

        return await _db.Duyurular
            .Where(d => d.IsActive && (
                d.Hedef == DuyuruHedef.Herkes ||
                d.Hedef == DuyuruHedef.Ogretmenler ||
                (d.Hedef == DuyuruHedef.DersOzeli && d.HedefDersAtamaId.HasValue
                    && ogretmenDersAtamaIdler.Contains(d.HedefDersAtamaId.Value))
            ))
            .Include(d => d.Yayinlayan)
            .Include(d => d.HedefDersAtama).ThenInclude(da => da!.Ders)
            .OrderByDescending(d => d.Onemli)
            .ThenByDescending(d => d.CreatedAt)
            .Select(d => new DuyuruOlusturViewModel
            {
                Id = d.Id,
                Baslik = d.Baslik,
                Icerik = d.Icerik,
                HedefAdi = d.Hedef.ToString(),
                Onemli = d.Onemli,
                YayinlayanAdi = d.Yayinlayan != null ? d.Yayinlayan.TamAd : string.Empty,
                DersAdi = d.HedefDersAtama != null && d.HedefDersAtama.Ders != null
                    ? d.HedefDersAtama.Ders.DersAdi : string.Empty,
                CreatedAt = d.CreatedAt
            })
            .ToListAsync();
    }

    // ─── SELECT LIST ─────────────────────────────────────────────────────────
    public async Task<IEnumerable<DersSelectViewModel>> GetDersAtamaSelectListAsync(int ogretmenId)
    {
        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);
        if (aktifDonem == null) return Enumerable.Empty<DersSelectViewModel>();

        return await _db.DersAtamalar
            .Where(da => da.OgretmenId == ogretmenId && da.DonemId == aktifDonem.Id && da.IsActive)
            .Include(da => da.Ders)
            .Select(da => new DersSelectViewModel
            {
                Id = da.Id,
                DisplayText = $"{da.Ders!.DersKodu} — {da.Ders.DersAdi}"
            })
            .ToListAsync();
    }

    // ─── PRIVATE: Harf Notu + GANO ────────────────────────────────────────────
    private static HarfNotu HesaplaHarfNotu(decimal genel)
    {
        double g = (double)genel;
        if (g >= AppConstants.Grading.AA_Esik) return HarfNotu.AA;
        if (g >= AppConstants.Grading.BA_Esik) return HarfNotu.BA;
        if (g >= AppConstants.Grading.BB_Esik) return HarfNotu.BB;
        if (g >= AppConstants.Grading.CB_Esik) return HarfNotu.CB;
        if (g >= AppConstants.Grading.CC_Esik) return HarfNotu.CC;
        if (g >= AppConstants.Grading.DC_Esik) return HarfNotu.DC;
        if (g >= AppConstants.Grading.DD_Esik) return HarfNotu.DD;
        return HarfNotu.FF;
    }

    private async Task GanoGuncelleAsync(int dersAtamaId)
    {
        try
        {
            var ogrenciIdler = await _db.OgrenciDersler
                .Where(od => od.DersAtamaId == dersAtamaId && od.IsActive)
                .Select(od => od.OgrenciId)
                .Distinct()
                .ToListAsync();

            foreach (var ogrenciId in ogrenciIdler)
            {
                var tamamlananDersler = await _db.OgrenciDersler
                    .Where(od => od.OgrenciId == ogrenciId
                              && od.Durum == OgrenciDersDurum.Tamamlandi
                              && od.HarfNotu.HasValue
                              && od.IsActive)
                    .Include(od => od.DersAtama).ThenInclude(da => da!.Ders)
                    .ToListAsync();

                if (!tamamlananDersler.Any()) continue;

                var katsayiMap = new Dictionary<HarfNotu, double>
                {
                    { HarfNotu.AA, AppConstants.NotKatsayisi.AA },
                    { HarfNotu.BA, AppConstants.NotKatsayisi.BA },
                    { HarfNotu.BB, AppConstants.NotKatsayisi.BB },
                    { HarfNotu.CB, AppConstants.NotKatsayisi.CB },
                    { HarfNotu.CC, AppConstants.NotKatsayisi.CC },
                    { HarfNotu.DC, AppConstants.NotKatsayisi.DC },
                    { HarfNotu.DD, AppConstants.NotKatsayisi.DD },
                    { HarfNotu.FF, AppConstants.NotKatsayisi.FF }
                };

                double toplamAktsKatsayi = 0;
                int toplamAkts = 0;

                foreach (var od in tamamlananDersler)
                {
                    var akts = od.DersAtama?.Ders?.Akts ?? 0;
                    if (akts == 0) continue;

                    var katsayi = katsayiMap.TryGetValue(od.HarfNotu!.Value, out var k) ? k : 0;
                    toplamAktsKatsayi += akts * katsayi;
                    toplamAkts += akts;
                }

                if (toplamAkts == 0) continue;

                var ogrenci = await _db.Ogrenciler.FindAsync(ogrenciId);
                if (ogrenci == null) continue;

                ogrenci.Gano = (decimal)Math.Round(toplamAktsKatsayi / toplamAkts, 2);
                ogrenci.TamamlananAkts = toplamAkts;
                ogrenci.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GANO güncelleme hatası: DersAtamaId={Id}", dersAtamaId);
        }
    }
}
