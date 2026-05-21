using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentManagement.Core.Constants;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;
using StudentManagement.Data.Context;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.ViewModels.Ogrenci;
using StudentManagement.Services.ViewModels.Ogretmen;
using StudentManagement.Services.Helpers;

public class BelgeTalebiCreateViewModel : BelgeTalebiOlusturViewModel { }

public class OgrenciPanelService : IOgrenciPanelService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IAuditLogWriter _audit;
    private readonly ILogger<OgrenciPanelService> _logger;

    public OgrenciPanelService(ApplicationDbContext db, IMapper mapper,
        IAuditLogWriter audit, ILogger<OgrenciPanelService> logger)
    {
        _db = db;
        _mapper = mapper;
        _audit = audit;
        _logger = logger;
    }

    // ─── DASHBOARD ────────────────────────────────────────────────────────────
    public async Task<OgrenciDashboardViewModel> GetDashboardAsync(int ogrenciId)
    {
        var ogrenci = await _db.Ogrenciler
            .Include(o => o.Kullanici)
            .Include(o => o.Bolum)
            .FirstOrDefaultAsync(o => o.Id == ogrenciId);

        if (ogrenci == null) return new OgrenciDashboardViewModel();

        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);

        var aktifDersler = await _db.OgrenciDersler
            .Where(od => od.OgrenciId == ogrenciId &&
                         od.Durum == OgrenciDersDurum.Devam && od.IsActive)
            .Include(od => od.DersAtama).ThenInclude(da => da!.Ders)
            .Include(od => od.DersAtama).ThenInclude(da => da!.Ogretmen)
            .Select(od => new OgrenciDersViewModel
            {
                OgrenciDersId = od.Id,
                DersAtamaId = od.DersAtama!.Id,
                DersAdi = od.DersAtama.Ders!.DersAdi,
                DersKodu = od.DersAtama.Ders.DersKodu,
                Akts = od.DersAtama.Ders.Akts,
                OgretmenAdi = od.DersAtama.Ogretmen != null ? od.DersAtama.Ogretmen.UnvanliAd : string.Empty,
                Gun = od.DersAtama.Gun.ToString(),
                BaslangicSaati = od.DersAtama.BaslangicSaati,
                BitisSaati = od.DersAtama.BitisSaati,
                Derslik = od.DersAtama.Derslik,
                VizeNotu = od.VizeNotu,
                FinalNotu = od.FinalNotu,
                GenelNot = od.GenelNot,
                HarfNotuAdi = od.HarfNotu.HasValue ? od.HarfNotu.Value.ToString() : "-",
                DurumAdi = od.Durum.ToString()
            })
            .ToListAsync();

        var yaklasanSinavlar = await _db.Sinavlar
            .Where(s => s.DersAtama!.OgrenciDersler!.Any(od =>
                    od.OgrenciId == ogrenciId && od.Durum == OgrenciDersDurum.Devam && od.IsActive)
                && s.SinavTarihi >= DateTime.Now
                && s.SinavTarihi <= DateTime.Now.AddDays(14)
                && s.IsActive)
            .Include(s => s.DersAtama).ThenInclude(da => da!.Ders)
            .OrderBy(s => s.SinavTarihi)
            .Take(5)
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

        return new OgrenciDashboardViewModel
        {
            TamAd = ogrenci.Kullanici!.TamAd,
            OgrenciNo = ogrenci.OgrenciNo,
            BolumAdi = ogrenci.Bolum?.BolumAdi ?? string.Empty,
            AktifDonemAdi = aktifDonem != null ? $"{aktifDonem.Yil} {aktifDonem.DonemTur}" : "—",
            Gano = ogrenci.Gano,
            TamamlananAkts = ogrenci.TamamlananAkts,
            MinMezuniyetAkts = ogrenci.Bolum?.MinMezuniyetAkts ?? 240,
            AktifDersSayisi = aktifDersler.Count,
            AktifAktsYuku = aktifDersler.Sum(d => d.Akts),
            YaklasanSinavSayisi = yaklasanSinavlar.Count,
            AktifDersler = aktifDersler,
            YaklasanSinavlar = yaklasanSinavlar
        };
    }

    // ─── DERS KAYIT ───────────────────────────────────────────────────────────
    public async Task<DersKayitViewModel> GetDersKayitAsync(int ogrenciId)
    {
        var ogrenci = await _db.Ogrenciler.Include(o => o.Bolum).FirstOrDefaultAsync(o => o.Id == ogrenciId);
        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);

        var kayitAcik = aktifDonem != null
            && DateTime.Now >= aktifDonem.DersKayitBaslangic
            && DateTime.Now <= aktifDonem.DersKayitBitis;

        if (aktifDonem == null)
            return new DersKayitViewModel { KayitAcik = false, DonemAdi = "Aktif dönem yok" };

        // Mevcut kayıtlar / talepler
        var mevcutKayitlar = await _db.OgrenciDersler
            .Where(od => od.OgrenciId == ogrenciId && od.IsActive
                && (od.Durum == OgrenciDersDurum.Devam || od.Durum == OgrenciDersDurum.Talep))
            .Include(od => od.DersAtama).ThenInclude(da => da!.Ders)
            .ToListAsync();

        var kayitliDersAtamaIdler = mevcutKayitlar
            .Where(k => k.Durum == OgrenciDersDurum.Devam)
            .Select(k => k.DersAtamaId)
            .ToHashSet();

        var talepVerilmisDersAtamaIdler = mevcutKayitlar
            .Where(k => k.Durum == OgrenciDersDurum.Talep)
            .Select(k => k.DersAtamaId)
            .ToHashSet();

        var mevcutAktsYuku = mevcutKayitlar
            .Where(k => k.Durum == OgrenciDersDurum.Devam)
            .Sum(k => k.DersAtama?.Ders?.Akts ?? 0);

        // Bu döneme ait tüm atamalar
        var dersler = await _db.DersAtamalar
            .Where(da => da.DonemId == aktifDonem.Id && da.IsActive)
            .Include(da => da.Ders).ThenInclude(d => d!.Bolum)
            .Include(da => da.Ogretmen)
            .OrderBy(da => da.Ders!.DersKodu)
            .Select(da => new DersKayitSatirViewModel
            {
                DersAtamaId = da.Id,
                DersKodu = da.Ders!.DersKodu,
                DersAdi = da.Ders.DersAdi,
                BolumAdi = da.Ders.Bolum != null ? da.Ders.Bolum.BolumAdi : string.Empty,
                OgretmenAdi = da.Ogretmen != null ? da.Ogretmen.UnvanliAd : string.Empty,
                Akts = da.Ders.Akts,
                Gun = da.Gun.ToString(),
                BaslangicSaati = da.BaslangicSaati,
                BitisSaati = da.BitisSaati,
                Derslik = da.Derslik,
                MaxKontenjan = da.Ders.MaxKontenjan,
                KayitliSayisi = da.KayitliOgrenciSayisi,
                ZatenKayitli = kayitliDersAtamaIdler.Contains(da.Id),
                TalepVar = talepVerilmisDersAtamaIdler.Contains(da.Id)
            })
            .ToListAsync();

        return new DersKayitViewModel
        {
            DonemAdi = $"{aktifDonem.Yil} {aktifDonem.DonemTur}",
            KayitAcik = kayitAcik,
            KayitBaslangic = aktifDonem.DersKayitBaslangic,
            KayitBitis = aktifDonem.DersKayitBitis,
            MevcutAktsYuku = mevcutAktsYuku,
            MevcutDersler = dersler
        };
    }

    public async Task<ServiceResult> DersKayitTalepAsync(int dersAtamaId, int ogrenciId, int userId)
    {
        // Aktif dönem kontrolü
        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);
        if (aktifDonem == null) return ServiceResult.Fail(AppConstants.ErrorMessages.AktifDonemYok);

        var kayitAcik = DateTime.Now >= aktifDonem.DersKayitBaslangic
                     && DateTime.Now <= aktifDonem.DersKayitBitis;
        if (!kayitAcik) return ServiceResult.Fail(AppConstants.ErrorMessages.DersKayitKapali);

        // Zaten kayıtlı mı?
        var mevcutKayit = await _db.OgrenciDersler
            .IgnoreQueryFilters()
            .AnyAsync(od => od.OgrenciId == ogrenciId && od.DersAtamaId == dersAtamaId
                         && (od.Durum == OgrenciDersDurum.Devam || od.Durum == OgrenciDersDurum.Talep));

        if (mevcutKayit) return ServiceResult.Fail(AppConstants.ErrorMessages.ZatenKayitli);

        // Kontenjan kontrolü
        var da = await _db.DersAtamalar
            .Include(x => x.Ders)
            .FirstOrDefaultAsync(x => x.Id == dersAtamaId && x.IsActive);

        if (da == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);
        if (da.KayitliOgrenciSayisi >= da.Ders!.MaxKontenjan)
            return ServiceResult.Fail(AppConstants.ErrorMessages.KontenjanDolu);

        var ogrenciDers = new OgrenciDers
        {
            OgrenciId = ogrenciId,
            DersAtamaId = dersAtamaId,
            Durum = OgrenciDersDurum.Talep,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = userId
        };

        _db.OgrenciDersler.Add(ogrenciDers);
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.DersKayitTalep, "OgrenciDers", ogrenciDers.Id);
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DersBirakAsync(int dersAtamaId, int ogrenciId, int userId)
    {
        var od = await _db.OgrenciDersler
            .Include(x => x.DersAtama)
            .FirstOrDefaultAsync(x => x.OgrenciId == ogrenciId
                               && x.DersAtamaId == dersAtamaId
                               && x.IsActive);

        if (od == null) return ServiceResult.Fail("Ders kaydı bulunamadı.");

        if (od.Durum == OgrenciDersDurum.Devam && od.DersAtama != null)
        {
            od.DersAtama.KayitliOgrenciSayisi =
                Math.Max(0, od.DersAtama.KayitliOgrenciSayisi - 1);
            od.DersAtama.UpdatedAt = DateTime.UtcNow;
        }

        od.Durum = OgrenciDersDurum.Cekildi;
        od.IsActive = false;
        od.UpdatedAt = DateTime.UtcNow;
        od.UpdatedBy = userId;

        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Update, "OgrenciDers", od.Id,
            aciklama: "Dersten çekildi");
        return ServiceResult.Ok();
    }

    // ─── DERSLERİM ────────────────────────────────────────────────────────────
    public async Task<IEnumerable<OgrenciDersViewModel>> GetDerslerimAsync(int ogrenciId)
    {
        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);
        if (aktifDonem == null) return Enumerable.Empty<OgrenciDersViewModel>();

        return await _db.OgrenciDersler
            .Where(od => od.OgrenciId == ogrenciId && od.IsActive
                && (od.Durum == OgrenciDersDurum.Devam || od.Durum == OgrenciDersDurum.Talep
                    || od.Durum == OgrenciDersDurum.Reddedildi))
            .Include(od => od.DersAtama).ThenInclude(da => da!.Ders)
            .Include(od => od.DersAtama).ThenInclude(da => da!.Ogretmen)
            .OrderBy(od => od.DersAtama!.Gun).ThenBy(od => od.DersAtama!.BaslangicSaati)
            .Select(od => new OgrenciDersViewModel
            {
                OgrenciDersId = od.Id,
                DersAtamaId = od.DersAtama!.Id,
                DersAdi = od.DersAtama.Ders!.DersAdi,
                DersKodu = od.DersAtama.Ders.DersKodu,
                Akts = od.DersAtama.Ders.Akts,
                OgretmenAdi = od.DersAtama.Ogretmen != null ? od.DersAtama.Ogretmen.UnvanliAd : string.Empty,
                Gun = od.DersAtama.Gun.ToString(),
                BaslangicSaati = od.DersAtama.BaslangicSaati,
                BitisSaati = od.DersAtama.BitisSaati,
                Derslik = od.DersAtama.Derslik,
                VizeNotu = od.VizeNotu,
                FinalNotu = od.FinalNotu,
                GenelNot = od.GenelNot,
                HarfNotuAdi = od.HarfNotu.HasValue ? od.HarfNotu.Value.ToString() : "-",
                DurumAdi = od.Durum.ToString()
            })
            .ToListAsync();
    }

    // ─── TRANSKRİPT ──────────────────────────────────────────────────────────
    public async Task<TranskriptViewModel> GetTranskriptAsync(int ogrenciId)
    {
        var ogrenci = await _db.Ogrenciler
            .Include(o => o.Kullanici)
            .Include(o => o.Bolum)
            .FirstOrDefaultAsync(o => o.Id == ogrenciId);

        if (ogrenci == null) return new TranskriptViewModel();

        var tamamlananDersler = await _db.OgrenciDersler
            .Where(od => od.OgrenciId == ogrenciId
                      && od.Durum == OgrenciDersDurum.Tamamlandi && od.IsActive)
            .Include(od => od.DersAtama).ThenInclude(da => da!.Ders)
            .Include(od => od.DersAtama).ThenInclude(da => da!.Donem)
            .OrderBy(od => od.DersAtama!.Donem!.Yil)
            .ThenBy(od => od.DersAtama!.Donem!.DonemTur)
            .ToListAsync();

        // Dönem grupları
        var donemGruplari = tamamlananDersler
            .GroupBy(od => new
            {
                od.DersAtama!.Donem!.Id,
                DonemAdi = $"{od.DersAtama.Donem.Yil} {od.DersAtama.Donem.DonemTur}"
            })
            .Select(g =>
            {
                var dersler = g.Select(od => new TranskriptSatirViewModel
                {
                    DersKodu = od.DersAtama!.Ders!.DersKodu,
                    DersAdi = od.DersAtama.Ders.DersAdi,
                    Akts = od.DersAtama.Ders.Akts,
                    DonemAdi = g.Key.DonemAdi,
                    VizeNotu = od.VizeNotu,
                    FinalNotu = od.FinalNotu,
                    GenelNot = od.GenelNot,
                    HarfNotuAdi = od.HarfNotu.HasValue ? od.HarfNotu.Value.ToString() : "-"
                }).ToList();

                var toplamAkts = dersler.Sum(d => d.Akts);

                // Dönem GANO
                decimal donemGano = 0;
                var katsayiMap = BuildKatsayiMap();
                decimal toplam = 0;
                foreach (var d in dersler)
                {
                    if (d.HarfNotuAdi == "-") continue;
                    if (!Enum.TryParse<HarfNotu>(d.HarfNotuAdi, out var harf)) continue;
                    var k = katsayiMap.GetValueOrDefault(harf, 0m);
                    toplam += d.Akts * k;
                }
                donemGano = toplamAkts > 0 ? Math.Round(toplam / toplamAkts, 2) : 0;

                return new TranskriptDonemGrubuViewModel
                {
                    DonemAdi = g.Key.DonemAdi,
                    ToplamAkts = toplamAkts,
                    DonemGanosu = donemGano,
                    Dersler = dersler
                };
            })
            .ToList();

        return new TranskriptViewModel
        {
            TamAd = ogrenci.Kullanici!.TamAd,
            OgrenciNo = ogrenci.OgrenciNo,
            BolumAdi = ogrenci.Bolum?.BolumAdi ?? string.Empty,
            Gano = ogrenci.Gano,
            TamamlananAkts = ogrenci.TamamlananAkts,
            TamamlananDersSayisi = tamamlananDersler.Count,
            MinMezuniyetAkts = ogrenci.Bolum?.MinMezuniyetAkts ?? 240,
            DonemGruplari = donemGruplari
        };
    }

    // ─── DERS PROGRAMI ────────────────────────────────────────────────────────
    public async Task<DersProgramiViewModel> GetDersProgramiAsync(int ogrenciId)
    {
        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);
        if (aktifDonem == null) return new DersProgramiViewModel { DonemAdi = "—" };

        var dersler = await _db.OgrenciDersler
            .Where(od => od.OgrenciId == ogrenciId
                      && od.Durum == OgrenciDersDurum.Devam && od.IsActive)
            .Include(od => od.DersAtama).ThenInclude(da => da!.Ders)
            .Include(od => od.DersAtama).ThenInclude(da => da!.Ogretmen)
            .Select(od => new DersProgramiSatirViewModel
            {
                DersAtamaId = od.DersAtama!.Id,
                DersAdi = od.DersAtama.Ders!.DersAdi,
                DersKodu = od.DersAtama.Ders.DersKodu,
                Akts = od.DersAtama.Ders.Akts,
                OgretmenAdi = od.DersAtama.Ogretmen != null ? od.DersAtama.Ogretmen.UnvanliAd : string.Empty,
                Gun = od.DersAtama.Gun.ToString(),
                GunSiraNo = (int)od.DersAtama.Gun,
                BaslangicSaati = od.DersAtama.BaslangicSaati,
                BitisSaati = od.DersAtama.BitisSaati,
                Derslik = od.DersAtama.Derslik
            })
            .ToListAsync();

        return new DersProgramiViewModel
        {
            DonemAdi = $"{aktifDonem.Yil} {aktifDonem.DonemTur}",
            Dersler = dersler
        };
    }

    // ─── SINAV TAKVİMİ ────────────────────────────────────────────────────────
    public async Task<SinavTakvimiViewModel> GetSinavTakvimiAsync(int ogrenciId)
    {
        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);
        if (aktifDonem == null) return new SinavTakvimiViewModel { DonemAdi = "—" };

        var sinavlar = await _db.Sinavlar
            .Where(s => s.DersAtama!.OgrenciDersler!.Any(od =>
                    od.OgrenciId == ogrenciId && od.Durum == OgrenciDersDurum.Devam && od.IsActive)
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
                Derslik = s.Derslik ?? string.Empty,
                Aciklama = s.Aciklama ?? string.Empty
            })
            .ToListAsync();

        return new SinavTakvimiViewModel
        {
            DonemAdi = $"{aktifDonem.Yil} {aktifDonem.DonemTur}",
            Sinavlar = sinavlar
        };
    }

    // ─── DUYURULAR ────────────────────────────────────────────────────────────
    public async Task<IEnumerable<DuyuruOlusturViewModel>> GetDuyurularAsync(int ogrenciId, int bolumId)
    {
        var kayitliDersAtamaIdler = await _db.OgrenciDersler
            .Where(od => od.OgrenciId == ogrenciId
                      && od.Durum == OgrenciDersDurum.Devam && od.IsActive)
            .Select(od => od.DersAtamaId)
            .ToListAsync();

        return await _db.Duyurular
            .Where(d => d.IsActive && (
                d.Hedef == DuyuruHedef.Herkes ||
                (d.Hedef == DuyuruHedef.TumOgrenciler) ||
                (d.Hedef == DuyuruHedef.Bolum && d.HedefBolumId == bolumId) ||
                (d.Hedef == DuyuruHedef.DersOzeli && d.HedefDersAtamaId.HasValue
                    && kayitliDersAtamaIdler.Contains(d.HedefDersAtamaId.Value))
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

    // ─── BELGELER ────────────────────────────────────────────────────────────
    public async Task<BelgelerViewModel> GetBelgelerAsync(int ogrenciId)
    {
        var talepler = await _db.BelgeTalepleri
            .Where(bt => bt.OgrenciId == ogrenciId && bt.IsActive)
            .Include(bt => bt.IslemYapan)
            .OrderByDescending(bt => bt.CreatedAt)
            .Select(bt => new StudentManagement.Services.ViewModels.Ogrenci.BelgeTalebiViewModel
            {
                Id = bt.Id,
                BelgeTurAdi = bt.BelgeTur.ToString(),
                BelgeDurumAdi = bt.Durum.ToString(),
                Aciklama = bt.Aciklama,
                IslemYapanAdi = bt.IslemYapan != null ? bt.IslemYapan.TamAd : string.Empty,
                CreatedAt = bt.CreatedAt
            })
            .ToListAsync();

        return new BelgelerViewModel { Talepler = talepler };
    }

    public async Task<ServiceResult> BelgeTalepOlusturAsync(
        BelgeTalebiOlusturViewModel model, int ogrenciId, int userId)
    {
        // Bekleyen talep limiti
        var bekleyenSayi = await _db.BelgeTalepleri
            .CountAsync(bt => bt.OgrenciId == ogrenciId
                           && bt.Durum == BelgeDurum.Beklemede
                           && bt.IsActive);

        if (bekleyenSayi >= AppConstants.Belge.MaxTalepPerOgrenci)
            return ServiceResult.Fail($"En fazla {AppConstants.Belge.MaxTalepPerOgrenci} bekleyen belge talebiniz olabilir.");

        var talep = _mapper.Map<BelgeTalebi>(model);
        talep.OgrenciId = ogrenciId;
        talep.Durum = BelgeDurum.Beklemede;
        talep.CreatedBy = userId;

        _db.BelgeTalepleri.Add(talep);
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Create, "BelgeTalebi", talep.Id,
            yeniDeger: talep.BelgeTur.ToString());
        return ServiceResult.Ok();
    }

    // ─── PRIVATE ─────────────────────────────────────────────────────────────
    private static Dictionary<HarfNotu, decimal> BuildKatsayiMap() => new()
    {
        { HarfNotu.AA, (decimal)AppConstants.NotKatsayisi.AA },
        { HarfNotu.BA, (decimal)AppConstants.NotKatsayisi.BA },
        { HarfNotu.BB, (decimal)AppConstants.NotKatsayisi.BB },
        { HarfNotu.CB, (decimal)AppConstants.NotKatsayisi.CB },
        { HarfNotu.CC, (decimal)AppConstants.NotKatsayisi.CC },
        { HarfNotu.DC, (decimal)AppConstants.NotKatsayisi.DC },
        { HarfNotu.DD, (decimal)AppConstants.NotKatsayisi.DD },
        { HarfNotu.FF, (decimal)AppConstants.NotKatsayisi.FF }
    };
    public class BelgeTalebiCreateViewModel : BelgeTalebiOlusturViewModel { }
}
