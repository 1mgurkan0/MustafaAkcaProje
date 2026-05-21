using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentManagement.Core.Constants;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;
using StudentManagement.Data.Context;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.ViewModels.Admin;
using StudentManagement.Services.ViewModels.OgrenciIsleri;


namespace StudentManagement.Services.Service;

public class OgrenciIsleriService : IOgrenciIsleriService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IAuditLogWriter _audit;
    private readonly ILogger<OgrenciIsleriService> _logger;

    public OgrenciIsleriService(ApplicationDbContext db, IMapper mapper,
        IAuditLogWriter audit, ILogger<OgrenciIsleriService> logger)
    {
        _db = db;
        _mapper = mapper;
        _audit = audit;
        _logger = logger;
    }

    // ─── DASHBOARD ────────────────────────────────────────────────────────────
    public async Task<OgrenciIsleriDashboardViewModel> GetDashboardAsync()
    {
        var bugun = DateTime.Today;
        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);

        var sonTalepler = await _db.OgrenciDersler
            .Where(od => od.Durum == OgrenciDersDurum.Talep && od.IsActive)
            .OrderByDescending(od => od.CreatedAt)
            .Take(AppConstants.Pagination.DashboardTopCount)
            .Select(od => new StudentManagement.Services.ViewModels.OgrenciIsleri.TalepOzetViewModel
            {
                OgrenciDersId = od.Id,
                OgrenciAdi = od.Ogrenci!.Kullanici!.TamAd,
                OgrenciNo = od.Ogrenci.OgrenciNo,
                DersAdi = od.DersAtama!.Ders!.DersAdi,
                DersKodu = od.DersAtama.Ders.DersKodu,
                TalepTarihi = od.CreatedAt
            })
            .ToListAsync();

        var bekleyenTalepSayisi = await _db.OgrenciDersler.CountAsync(od => od.Durum == OgrenciDersDurum.Talep && od.IsActive);
        var bugunkuOnaylananSayisi = await _db.OgrenciDersler.CountAsync(od => od.Durum == OgrenciDersDurum.Devam && od.UpdatedAt.Date == bugun);
        var bekleyenBelgeSayisi = await _db.BelgeTalepleri.CountAsync(bt => bt.Durum == BelgeDurum.Beklemede && bt.IsActive);
        var toplamAktifOgrenci = await _db.Ogrenciler.CountAsync(o => o.Durum == OgrenciDurum.Aktif && o.IsActive);
        var aktifDonemAdi = aktifDonem != null ? $"{aktifDonem.Yil} {aktifDonem.DonemTur}" : "—";

        return new OgrenciIsleriDashboardViewModel
        {
            BekleyenKayitTalep = bekleyenTalepSayisi,
            BekleyenTalepSayisi = bekleyenTalepSayisi,
            BugunkuOnay = bugunkuOnaylananSayisi,
            BugunkuOnaylananSayisi = bugunkuOnaylananSayisi,
            BekleyenBelgeTalebi = bekleyenBelgeSayisi,
            BekleyenBelgeSayisi = bekleyenBelgeSayisi,
            ToplamAktifOgrenci = toplamAktifOgrenci,
            DonemAdi = aktifDonemAdi,
            AktifDonemAdi = aktifDonemAdi,
            SonTalepler = sonTalepler
        };
    }

    // ─── DERS KAYIT TALEPLERİ ────────────────────────────────────────────────
   public async Task<IEnumerable<StudentManagement.Services.ViewModels.OgrenciIsleri.TalepDetayViewModel>> GetTaleplerAsync()
    {
        return await _db.OgrenciDersler
            .Where(od => od.IsActive &&
                (od.Durum == OgrenciDersDurum.Talep || od.Durum == OgrenciDersDurum.Devam || od.Durum == OgrenciDersDurum.Reddedildi))
            .OrderByDescending(od => od.CreatedAt)
            .Select(od => new StudentManagement.Services.ViewModels.OgrenciIsleri.TalepDetayViewModel
            {
                OgrenciDersId = od.Id,
                OgrenciAdi = od.Ogrenci!.Kullanici!.TamAd,
                OgrenciNo = od.Ogrenci.OgrenciNo,
                BolumKodu = od.Ogrenci.Bolum != null ? od.Ogrenci.Bolum.BolumKodu : string.Empty,
                DersAdi = od.DersAtama!.Ders!.DersAdi,
                DersKodu = od.DersAtama.Ders.DersKodu,
                Akts = od.DersAtama.Ders.Akts,
                OgretmenAdi = od.DersAtama.Ogretmen != null ? od.DersAtama.Ogretmen.TamAd : string.Empty,
                TalepTarihi = od.CreatedAt,
                DurumAdi = od.Durum.ToString()
            })
            .ToListAsync();
    }

    public async Task<ServiceResult<StudentManagement.Services.ViewModels.OgrenciIsleri.TalepDetayViewModel>> GetTalepDetayAsync(int ogrenciDersId)
    {
        var od = await _db.OgrenciDersler
            .Include(x => x.Ogrenci).ThenInclude(o => o!.Kullanici)
            .Include(x => x.Ogrenci).ThenInclude(o => o!.Bolum)
            .Include(x => x.DersAtama).ThenInclude(da => da!.Ders)
            .Include(x => x.DersAtama).ThenInclude(da => da!.Ogretmen)
            .Include(x => x.DersAtama).ThenInclude(da => da!.Donem)
            .FirstOrDefaultAsync(x => x.Id == ogrenciDersId);

        if (od == null) return ServiceResult<StudentManagement.Services.ViewModels.OgrenciIsleri.TalepDetayViewModel>.Fail(AppConstants.ErrorMessages.KayitBulunamadi);

        var aktifAktsYuku = await _db.OgrenciDersler
            .Where(x => x.OgrenciId == od.OgrenciId &&
                        x.Durum == OgrenciDersDurum.Devam && x.IsActive)
            .SumAsync(x => x.DersAtama!.Ders!.Akts);

        var vm = new StudentManagement.Services.ViewModels.OgrenciIsleri.TalepDetayViewModel
        {
            OgrenciDersId = od.Id,
            OgrenciAdi = od.Ogrenci!.Kullanici!.TamAd,
            OgrenciNo = od.Ogrenci.OgrenciNo,
            BolumAdi = od.Ogrenci.Bolum?.BolumAdi ?? string.Empty,
            Gano = od.Ogrenci.Gano,
            MevcutAktsYuku = aktifAktsYuku,
            DersKodu = od.DersAtama!.Ders!.DersKodu,
            DersAdi = od.DersAtama.Ders.DersAdi,
            OgretmenAdi = od.DersAtama.Ogretmen?.TamAd ?? string.Empty,
            Akts = od.DersAtama.Ders.Akts,
            Gun = od.DersAtama.Gun.ToString(),
            BaslangicSaati = od.DersAtama.BaslangicSaati,
            BitisSaati = od.DersAtama.BitisSaati,
            Derslik = od.DersAtama.Derslik,
            MaxKontenjan = od.DersAtama.Ders.MaxKontenjan,
            KayitliSayisi = od.DersAtama.KayitliOgrenciSayisi,
            DurumAdi = od.Durum.ToString()
        };

        return ServiceResult<StudentManagement.Services.ViewModels.OgrenciIsleri.TalepDetayViewModel>.Ok(vm);
    }

    public async Task<ServiceResult> TalepOnaylaAsync(int ogrenciDersId, int userId)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var od = await _db.OgrenciDersler
                .Include(x => x.DersAtama).ThenInclude(da => da!.Ders)
                .FirstOrDefaultAsync(x => x.Id == ogrenciDersId);

            if (od == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);
            if (od.Durum != OgrenciDersDurum.Talep) return ServiceResult.Fail("Bu talep zaten işlenmiş.");

            // Kontenjan kontrolü
            if (od.DersAtama!.KayitliOgrenciSayisi >= od.DersAtama.Ders!.MaxKontenjan)
                return ServiceResult.Fail(AppConstants.ErrorMessages.KontenjanDolu);

            od.Durum = OgrenciDersDurum.Devam;
            od.UpdatedAt = DateTime.UtcNow;
            od.UpdatedBy = userId;

            od.DersAtama.KayitliOgrenciSayisi++;
            od.DersAtama.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            await _audit.WriteAsync(userId, AuditAction.DersKayitOnayla, "OgrenciDers", ogrenciDersId);
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            _logger.LogError(ex, "TalepOnayla hatası: {Id}", ogrenciDersId);
            return ServiceResult.Fail(AppConstants.ErrorMessages.GenelHata);
        }
    }

    public async Task<ServiceResult> TalepReddetAsync(int ogrenciDersId, string redNedeni, int userId)
    {
        var od = await _db.OgrenciDersler.FindAsync(ogrenciDersId);
        if (od == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);
        if (od.Durum != OgrenciDersDurum.Talep) return ServiceResult.Fail("Bu talep zaten işlenmiş.");

        od.Durum = OgrenciDersDurum.Reddedildi;
        od.RedNedeni = redNedeni;
        od.UpdatedAt = DateTime.UtcNow;
        od.UpdatedBy = userId;

        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.DersKayitReddet, "OgrenciDers", ogrenciDersId,
            aciklama: redNedeni);
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult<int>> TopluOnaylaAsync(List<int> idler, int userId)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var talepler = await _db.OgrenciDersler
                .Include(x => x.DersAtama).ThenInclude(da => da!.Ders)
                .Where(od => idler.Contains(od.Id) && od.Durum == OgrenciDersDurum.Talep && od.IsActive)
                .ToListAsync();

            int onaylanan = 0;
            foreach (var od in talepler)
            {
                if (od.DersAtama!.KayitliOgrenciSayisi >= od.DersAtama.Ders!.MaxKontenjan) continue;

                od.Durum = OgrenciDersDurum.Devam;
                od.UpdatedAt = DateTime.UtcNow;
                od.UpdatedBy = userId;
                od.DersAtama.KayitliOgrenciSayisi++;
                od.DersAtama.UpdatedAt = DateTime.UtcNow;
                onaylanan++;
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            await _audit.WriteAsync(userId, AuditAction.DersKayitOnayla, "OgrenciDers",
                aciklama: $"Toplu onay: {onaylanan} talep");
            return ServiceResult<int>.Ok(onaylanan);
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            _logger.LogError(ex, "TopluOnayla hatası");
            return ServiceResult<int>.Fail(AppConstants.ErrorMessages.GenelHata);
        }
    }

    // ─── BELGE TALEPLERİ ─────────────────────────────────────────────────────
    public async Task<IEnumerable<StudentManagement.Services.ViewModels.OgrenciIsleri.BelgeTalebiViewModel>> GetBelgeTalepleriAsync()
    {
        return await _db.BelgeTalepleri
            .Where(bt => bt.IsActive)
            .Include(bt => bt.Ogrenci).ThenInclude(o => o!.Kullanici)
            .Include(bt => bt.IslemYapan)
            .OrderByDescending(bt => bt.CreatedAt)
            .Select(bt => new StudentManagement.Services.ViewModels.OgrenciIsleri.BelgeTalebiViewModel
            {
                Id = bt.Id,
                OgrenciAdi = bt.Ogrenci!.Kullanici!.TamAd,
                OgrenciNo = bt.Ogrenci.OgrenciNo,
                BelgeTurAdi = bt.BelgeTur.ToString(),
                BelgeDurumAdi = bt.Durum.ToString(),
                Aciklama = bt.Aciklama,
                IslemYapanAdi = bt.IslemYapan != null ? bt.IslemYapan.TamAd : string.Empty,
                CreatedAt = bt.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ServiceResult> BelgeDurumGuncelleAsync(BelgeDurumGuncelleViewModel model, int userId)
    {
        var bt = await _db.BelgeTalepleri.FindAsync(model.Id);
        if (bt == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);

        bt.Durum = model.Durum;
        bt.IslemYapanId = userId;
        bt.UpdatedAt = DateTime.UtcNow;
        bt.UpdatedBy = userId;

        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Update, "BelgeTalebi", bt.Id,
            aciklama: $"Durum: {model.Durum}");
        return ServiceResult.Ok();
    }

    // ─── ÖĞRENCİ ARA ─────────────────────────────────────────────────────────
    public async Task<List<OgrenciIsleriOgrenciViewModel>> OgrenciAraAsync(string? q, int? bolumId)
    {
        q = (q ?? string.Empty).ToLower().Trim();

        var query = _db.Ogrenciler
            .Where(o => o.IsActive)
            .Include(o => o.Kullanici)
            .Include(o => o.Bolum)
            .AsQueryable();

        if (!string.IsNullOrEmpty(q))
            query = query.Where(o =>
                o.OgrenciNo.Contains(q) ||
                o.Kullanici!.Ad.ToLower().Contains(q) ||
                o.Kullanici.Soyad.ToLower().Contains(q) ||
                o.Kullanici.Email.ToLower().Contains(q) ||
                (o.Kullanici.Ad.ToLower() + " " + o.Kullanici.Soyad.ToLower()).Contains(q));

        if (bolumId.HasValue && bolumId > 0)
            query = query.Where(o => o.BolumId == bolumId);

        return await query
            .OrderBy(o => o.Kullanici!.Soyad)
            .Take(50)
            .Select(o => new OgrenciIsleriOgrenciViewModel
            {
                OgrenciId    = o.Id,
                OgrenciNo    = o.OgrenciNo,
                AdSoyad      = o.Kullanici!.TamAd,
                Email        = o.Kullanici.Email,
                BolumAdi     = o.Bolum != null ? o.Bolum.BolumAdi : string.Empty,
                Gano         = o.Gano,
                Durum        = o.Durum
            })
            .ToListAsync();
    }

    // ─── DUYURU ──────────────────────────────────────────────────────────────
    public async Task<ServiceResult> DuyuruOlusturAsync(DuyuruOlusturViewModel model, int userId)
    {
        var duyuru = _mapper.Map<Duyuru>(model);
        duyuru.YayinlayanId = userId;
        duyuru.CreatedBy = userId;

        _db.Duyurular.Add(duyuru);
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.DuyuruYayinla, "Duyuru", duyuru.Id,
            yeniDeger: duyuru.Baslik);
        return ServiceResult.Ok();
    }

    // ─── ÖĞRENCİ DETAY ───────────────────────────────────────────────────────
    public async Task<ServiceResult<StudentManagement.Services.ViewModels.Admin.OgrenciDetayViewModel>> OgrenciDetayAsync(int ogrenciId)
    {
        var ogrenci = await _db.Ogrenciler
            .Include(o => o.Kullanici)
            .Include(o => o.Bolum)
            .FirstOrDefaultAsync(o => o.Id == ogrenciId);

        if (ogrenci == null)
            return ServiceResult<StudentManagement.Services.ViewModels.Admin.OgrenciDetayViewModel>.Fail(AppConstants.ErrorMessages.KayitBulunamadi);

        var aktifDersSayisi = await _db.OgrenciDersler
            .CountAsync(od => od.OgrenciId == ogrenciId
                           && od.Durum == OgrenciDersDurum.Devam
                           && od.IsActive);

        var vm = new StudentManagement.Services.ViewModels.Admin.OgrenciDetayViewModel
        {
            OgrenciId = ogrenci.Id,
            OgrenciNo = ogrenci.OgrenciNo,
            TamAd = ogrenci.Kullanici!.TamAd,
            Email = ogrenci.Kullanici.Email,
            BolumAdi = ogrenci.Bolum?.BolumAdi ?? string.Empty,
            BolumKodu = ogrenci.Bolum?.BolumKodu ?? string.Empty,
            Gano = ogrenci.Gano,
            TamamlananAkts = ogrenci.TamamlananAkts,
            AktifDersSayisi = aktifDersSayisi,
            DurumAdi = ogrenci.Durum.ToString()
        };

        return ServiceResult<StudentManagement.Services.ViewModels.Admin.OgrenciDetayViewModel>.Ok(vm);
    }

    // ─── BÖLÜM SELECT LIST ────────────────────────────────────────────────────
    public async Task<IEnumerable<BolumSelectViewModel>> GetBolumSelectListAsync()
    {
        return await _db.Bolumler
            .Where(b => b.IsActive)
            .OrderBy(b => b.BolumAdi)
            .Select(b => new BolumSelectViewModel
            {
                Id = b.Id,
                DisplayText = $"{b.BolumKodu} — {b.BolumAdi}"
            })
            .ToListAsync();
    }
}
