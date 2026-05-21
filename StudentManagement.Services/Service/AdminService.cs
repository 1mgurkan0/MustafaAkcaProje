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

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IAuditLogWriter _audit;
    private readonly ILogger<AdminService> _logger;

    public AdminService(ApplicationDbContext db, IMapper mapper,
        IAuditLogWriter audit, ILogger<AdminService> logger)
    {
        _db = db;
        _mapper = mapper;
        _audit = audit;
        _logger = logger;
    }

    // ═══ DASHBOARD ═══════════════════════════════════════════════════════════
    public async Task<AdminDashboardViewModel> GetDashboardAsync()
    {
        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);

        var model = new AdminDashboardViewModel
        {
            ToplamOgrenci = await _db.Ogrenciler.CountAsync(o => o.IsActive),
            ToplamOgretmen = await _db.Kullanicilar.CountAsync(k => k.Rol == KullaniciRol.Ogretmen && k.IsActive),
            ToplamBolum = await _db.Bolumler.CountAsync(b => b.IsActive),
            ToplamDers = await _db.Dersler.CountAsync(d => d.IsActive),
            ToplamDersAtama = aktifDonem != null
                ? await _db.DersAtamalar.CountAsync(da => da.DonemId == aktifDonem.Id && da.IsActive)
                : 0,
            BekleyenTalepSayisi = await _db.OgrenciDersler
                .CountAsync(od => od.Durum == OgrenciDersDurum.Talep && od.IsActive),
            AktifDonemAdi = aktifDonem != null
                ? $"{aktifDonem.Yil} {aktifDonem.DonemTur}"
                : "Aktif dönem yok"
        };

        // Son talepler
        model.SonTalepler = await _db.OgrenciDersler
            .Where(od => od.Durum == OgrenciDersDurum.Talep && od.IsActive)
            .OrderByDescending(od => od.CreatedAt)
            .Take(AppConstants.Pagination.DashboardTopCount)
            .Select(od => new StudentManagement.Services.ViewModels.Admin.TalepOzetViewModel
            {
                OgrenciDersId = od.Id,
                OgrenciAdi = od.Ogrenci!.Kullanici!.TamAd,
                OgrenciNo = od.Ogrenci.OgrenciNo,
                DersAdi = od.DersAtama!.Ders!.DersAdi,
                DersKodu = od.DersAtama.Ders.DersKodu,
                TalepTarihi = od.CreatedAt
            })
            .ToListAsync();

        // Son öğrenciler
        model.SonOgrenciler = await _db.Ogrenciler
            .Where(o => o.IsActive)
            .Include(o => o.Kullanici)
            .Include(o => o.Bolum)
            .OrderByDescending(o => o.CreatedAt)
            .Take(AppConstants.Pagination.DashboardTopCount)
            .Select(o => new OgrenciViewModel
            {
                OgrenciId = o.Id,
                OgrenciNo = o.OgrenciNo,
                TamAd = o.Kullanici!.TamAd,
                Email = o.Kullanici.Email,
                BolumKodu = o.Bolum != null ? o.Bolum.BolumKodu : string.Empty,
                BolumAdi = o.Bolum != null ? o.Bolum.BolumAdi : string.Empty,
                Gano = o.Gano,
                DurumAdi = o.Durum.ToString()
            })
            .ToListAsync();

        // Bölüm istatistikleri
        model.BolumIstatistikleri = await _db.Bolumler
            .Where(b => b.IsActive)
            .Select(b => new BolumViewModel
            {
                Id = b.Id,
                BolumAdi = b.BolumAdi,
                BolumKodu = b.BolumKodu,
                OgrenciSayisi = b.Ogrenciler.Count(o => o.IsActive)
            })
            .OrderByDescending(b => b.OgrenciSayisi)
            .ToListAsync();

        return model;
    }

    // ═══ BÖLÜM ═══════════════════════════════════════════════════════════════
    public async Task<IEnumerable<BolumViewModel>> GetBolumlerAsync()
    {
        return await _db.Bolumler
            .OrderBy(b => b.BolumAdi)
            .Select(b => new BolumViewModel
            {
                Id = b.Id,
                BolumKodu = b.BolumKodu,
                BolumAdi = b.BolumAdi,
                MinMezuniyetAkts = b.MinMezuniyetAkts,
                OgrenciSayisi = b.Ogrenciler.Count(o => o.IsActive),
                DersSayisi = b.Dersler.Count(d => d.IsActive),
                IsActive = b.IsActive
            })
            .ToListAsync();
    }

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

    public async Task<ServiceResult<BolumDuzenleViewModel>> GetBolumDuzenleAsync(int id)
    {
        var bolum = await _db.Bolumler.FindAsync(id);
        if (bolum == null) return ServiceResult<BolumDuzenleViewModel>.Fail(AppConstants.ErrorMessages.KayitBulunamadi);
        return ServiceResult<BolumDuzenleViewModel>.Ok(_mapper.Map<BolumDuzenleViewModel>(bolum));
    }

    public async Task<ServiceResult> BolumOlusturAsync(BolumOlusturViewModel model, int userId)
    {
        if (await _db.Bolumler.AnyAsync(b => b.BolumKodu == model.BolumKodu))
            return ServiceResult.Fail("Bu bölüm kodu zaten kullanılıyor.");

        var bolum = _mapper.Map<Bolum>(model);
        bolum.CreatedBy = userId;
        _db.Bolumler.Add(bolum);
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Create, "Bolum", bolum.Id,
            yeniDeger: bolum.BolumAdi);
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> BolumGuncelleAsync(BolumDuzenleViewModel model, int userId)
    {
        var bolum = await _db.Bolumler.FindAsync(model.Id);
        if (bolum == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);

        var eski = bolum.BolumAdi;
        _mapper.Map(model, bolum);
        bolum.UpdatedAt = DateTime.UtcNow;
        bolum.UpdatedBy = userId;
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Update, "Bolum", bolum.Id, eski, bolum.BolumAdi);
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> BolumSilAsync(int id, int userId)
    {
        var bolum = await _db.Bolumler.FindAsync(id);
        if (bolum == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);

        if (await _db.Ogrenciler.AnyAsync(o => o.BolumId == id && o.IsActive))
            return ServiceResult.Fail("Bu bölümde aktif öğrenci bulunduğu için silinemez.");

        bolum.IsActive = false;
        bolum.UpdatedAt = DateTime.UtcNow;
        bolum.UpdatedBy = userId;
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Delete, "Bolum", id);
        return ServiceResult.Ok();
    }

    // ═══ DÖNEM ═══════════════════════════════════════════════════════════════
    public async Task<IEnumerable<DonemViewModel>> GetDonemlerAsync()
    {
        return await _db.Donemler
            .Include(d => d.DersAtamalari)
            .OrderByDescending(d => d.Yil).ThenBy(d => d.DonemTur)
            .Select(d => new DonemViewModel
            {
                Id = d.Id,
                DonemKodu = d.DonemKodu,
                Yil = d.Yil,
                DonemTurAdi = d.DonemTur.ToString(),
                AktifMi = d.AktifMi,
                IsActive = d.IsActive,
                DersKayitBaslangic = d.DersKayitBaslangic,
                DersKayitBitis = d.DersKayitBitis,
                DersAtamaSayisi = d.DersAtamalari != null ? d.DersAtamalari.Count(da => da.IsActive) : 0
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<DonemSelectViewModel>> GetDonemSelectListAsync()
    {
        return await _db.Donemler
            .Where(d => d.IsActive)
            .OrderByDescending(d => d.AktifMi)
            .Select(d => new DonemSelectViewModel
            {
                Id = d.Id,
                DisplayText = $"{d.Yil} {d.DonemTur}{(d.AktifMi ? " (Aktif)" : "")}"
            })
            .ToListAsync();
    }

    public async Task<ServiceResult<DonemDuzenleViewModel>> GetDonemDuzenleAsync(int id)
    {
        var donem = await _db.Donemler.FindAsync(id);
        if (donem == null) return ServiceResult<DonemDuzenleViewModel>.Fail(AppConstants.ErrorMessages.KayitBulunamadi);
        return ServiceResult<DonemDuzenleViewModel>.Ok(_mapper.Map<DonemDuzenleViewModel>(donem));
    }

    public async Task<ServiceResult> DonemOlusturAsync(DonemOlusturViewModel model, int userId)
    {
        if (await _db.Donemler.AnyAsync(d => d.DonemKodu == model.DonemKodu))
            return ServiceResult.Fail("Bu dönem kodu zaten kullanılıyor.");

        var donem = _mapper.Map<Donem>(model);
        donem.CreatedBy = userId;
        _db.Donemler.Add(donem);
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Create, "Donem", donem.Id, yeniDeger: donem.DonemKodu);
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DonemGuncelleAsync(DonemDuzenleViewModel model, int userId)
    {
        var donem = await _db.Donemler.FindAsync(model.Id);
        if (donem == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);
        _mapper.Map(model, donem);
        donem.UpdatedAt = DateTime.UtcNow;
        donem.UpdatedBy = userId;
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Update, "Donem", donem.Id);
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DonemAktifYapAsync(int id, int userId)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            await _db.Donemler.Where(d => d.AktifMi)
                .ExecuteUpdateAsync(s => s.SetProperty(d => d.AktifMi, false));

            var donem = await _db.Donemler.FindAsync(id);
            if (donem == null) { await tx.RollbackAsync(); return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi); }

            donem.AktifMi = true;
            donem.UpdatedAt = DateTime.UtcNow;
            donem.UpdatedBy = userId;
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            await _audit.WriteAsync(userId, AuditAction.Update, "Donem", id, aciklama: "Aktif dönem değiştirildi");
            return ServiceResult.Ok();
        }
        catch { await tx.RollbackAsync(); return ServiceResult.Fail(AppConstants.ErrorMessages.GenelHata); }
    }

    // ═══ DERS ═══════════════════════════════════════════════════════════════
    public async Task<IEnumerable<DersViewModel>> GetDersKataloguAsync()
    {
        return await _db.Dersler
            .Include(d => d.Bolum)
            .OrderBy(d => d.DersKodu)
            .Select(d => new DersViewModel
            {
                Id = d.Id,
                DersKodu = d.DersKodu,
                DersAdi = d.DersAdi,
                Akts = d.Akts,
                MaxKontenjan = d.MaxKontenjan,
                BolumAdi = d.Bolum != null ? d.Bolum.BolumAdi : string.Empty,
                BolumKodu = d.Bolum != null ? d.Bolum.BolumKodu : string.Empty,
                IsActive = d.IsActive
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<DersSelectViewModel>> GetDersSelectListAsync()
    {
        return await _db.Dersler
            .Where(d => d.IsActive)
            .Include(d => d.Bolum)
            .OrderBy(d => d.DersKodu)
            .Select(d => new DersSelectViewModel
            {
                Id = d.Id,
                DisplayText = $"{d.DersKodu} — {d.DersAdi} ({d.Akts} AKTS)"
            })
            .ToListAsync();
    }

    public async Task<ServiceResult<DersDuzenleViewModel>> GetDersDuzenleAsync(int id)
    {
        var ders = await _db.Dersler.FindAsync(id);
        if (ders == null) return ServiceResult<DersDuzenleViewModel>.Fail(AppConstants.ErrorMessages.KayitBulunamadi);
        var vm = _mapper.Map<DersDuzenleViewModel>(ders);
        vm.DersKodu = ders.DersKodu;
        return ServiceResult<DersDuzenleViewModel>.Ok(vm);
    }

    public async Task<ServiceResult> DersOlusturAsync(DersOlusturViewModel model, int userId)
    {
        if (await _db.Dersler.AnyAsync(d => d.DersKodu == model.DersKodu))
            return ServiceResult.Fail("Bu ders kodu zaten kullanılıyor.");

        var ders = _mapper.Map<Ders>(model);
        ders.CreatedBy = userId;
        _db.Dersler.Add(ders);
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Create, "Ders", ders.Id, yeniDeger: ders.DersAdi);
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DersGuncelleAsync(DersDuzenleViewModel model, int userId)
    {
        var ders = await _db.Dersler.FindAsync(model.Id);
        if (ders == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);
        _mapper.Map(model, ders);
        ders.UpdatedAt = DateTime.UtcNow;
        ders.UpdatedBy = userId;
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Update, "Ders", ders.Id);
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DersSilAsync(int id, int userId)
    {
        var ders = await _db.Dersler.FindAsync(id);
        if (ders == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);

        if (await _db.DersAtamalar.AnyAsync(da => da.DersId == id && da.IsActive))
            return ServiceResult.Fail("Bu dersin aktif ataması olduğu için silinemez.");

        ders.IsActive = false;
        ders.UpdatedAt = DateTime.UtcNow;
        ders.UpdatedBy = userId;
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Delete, "Ders", id);
        return ServiceResult.Ok();
    }

    // ═══ DERS ATAMA ══════════════════════════════════════════════════════════
    public async Task<IEnumerable<DersAtamaViewModel>> GetDersAtamalarAsync()
    {
        var aktifDonem = await _db.Donemler.FirstOrDefaultAsync(d => d.AktifMi && d.IsActive);
        if (aktifDonem == null) return Enumerable.Empty<DersAtamaViewModel>();

        return await _db.DersAtamalar
            .Where(da => da.DonemId == aktifDonem.Id && da.IsActive)
            .Include(da => da.Ders).ThenInclude(d => d!.Bolum)
            .Include(da => da.Ogretmen)
            .Include(da => da.Donem)
            .OrderBy(da => da.Ders!.DersKodu)
            .Select(da => new DersAtamaViewModel
            {
                Id = da.Id,
                DersAdi = da.Ders!.DersAdi,
                DersKodu = da.Ders.DersKodu,
                Akts = da.Ders.Akts,
                MaxKontenjan = da.Ders.MaxKontenjan,
                OgretmenAdi = da.Ogretmen!.TamAd,
                OgretmenUnvanliAd = da.Ogretmen.UnvanliAd,
                DonemAdi = $"{da.Donem!.Yil} {da.Donem.DonemTur}",
                BolumAdi = da.Ders.Bolum != null ? da.Ders.Bolum.BolumAdi : string.Empty,
                GunAdi = da.Gun.ToString(),
                BaslangicSaati = da.BaslangicSaati,
                BitisSaati = da.BitisSaati,
                Derslik = da.Derslik,
                KayitliOgrenciSayisi = da.KayitliOgrenciSayisi,
                DolulukOrani = da.Ders.MaxKontenjan > 0
                    ? (double)da.KayitliOgrenciSayisi / da.Ders.MaxKontenjan * 100 : 0
            })
            .ToListAsync();
    }

    public async Task<ServiceResult<DersAtamaDetayViewModel>> GetDersAtamaDetayAsync(int id)
    {
        var da = await _db.DersAtamalar
            .Include(x => x.Ders).ThenInclude(d => d!.Bolum)
            .Include(x => x.Ogretmen)
            .Include(x => x.Donem)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (da == null) return ServiceResult<DersAtamaDetayViewModel>.Fail(AppConstants.ErrorMessages.KayitBulunamadi);

        var ogrenciler = await _db.OgrenciDersler
            .Where(od => od.DersAtamaId == id && od.IsActive)
            .Include(od => od.Ogrenci).ThenInclude(o => o!.Kullanici)
            .Include(od => od.Ogrenci).ThenInclude(o => o!.Bolum)
            .Select(od => new DersAtamaOgrenciViewModel
            {
                OgrenciId = od.OgrenciId,
                OgrenciNo = od.Ogrenci!.OgrenciNo,
                OgrenciAdi = od.Ogrenci.Kullanici!.TamAd,
                BolumKodu = od.Ogrenci.Bolum != null ? od.Ogrenci.Bolum.BolumKodu : string.Empty,
                VizeNotu = od.VizeNotu,
                FinalNotu = od.FinalNotu,
                GenelNot = od.GenelNot,
                HarfNotuAdi = od.HarfNotu.HasValue ? od.HarfNotu.Value.ToString() : "—",
                DurumAdi = od.Durum.ToString()
            })
            .ToListAsync();

        var vm = new DersAtamaDetayViewModel
        {
            DersAtamaId = da.Id,
            DersAdi = da.Ders!.DersAdi,
            DersKodu = da.Ders.DersKodu,
            Akts = da.Ders.Akts,
            MaxKontenjan = da.Ders.MaxKontenjan,
            OgretmenUnvanliAd = da.Ogretmen!.UnvanliAd,
            DonemAdi = $"{da.Donem!.Yil} {da.Donem.DonemTur}",
            GunAdi = da.Gun.ToString(),
            BaslangicSaati = da.BaslangicSaati,
            BitisSaati = da.BitisSaati,
            Derslik = da.Derslik,
            KayitliOgrenciSayisi = da.KayitliOgrenciSayisi,
            KayitliOgrenciler = ogrenciler
        };

        return ServiceResult<DersAtamaDetayViewModel>.Ok(vm);
    }

    public async Task<ServiceResult> DersAtamaOlusturAsync(DersAtamaOlusturViewModel model, int userId)
    {
        // Aynı dönemde aynı ders zaten atanmış mı?
        if (await _db.DersAtamalar.AnyAsync(da =>
            da.DersId == model.DersId && da.DonemId == model.DonemId && da.IsActive))
            return ServiceResult.Fail("Bu ders bu dönem için zaten atanmış.");

        var atama = _mapper.Map<DersAtama>(model);
        atama.CreatedBy = userId;
        _db.DersAtamalar.Add(atama);
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Create, "DersAtama", atama.Id);
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DersAtamaSilAsync(int id, int userId)
    {
        var atama = await _db.DersAtamalar.FindAsync(id);
        if (atama == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);

        if (await _db.OgrenciDersler.AnyAsync(od => od.DersAtamaId == id && od.IsActive))
            return ServiceResult.Fail("Bu derse kayıtlı öğrenci bulunduğu için silinemez.");

        atama.IsActive = false;
        atama.UpdatedAt = DateTime.UtcNow;
        atama.UpdatedBy = userId;
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Delete, "DersAtama", id);
        return ServiceResult.Ok();
    }

    public async Task<IEnumerable<KullaniciOzetViewModel>> GetOgretmenSelectListAsync()
    {
        return await _db.Kullanicilar
            .Where(k => k.Rol == KullaniciRol.Ogretmen && k.IsActive)
            .OrderBy(k => k.Soyad)
            .Select(k => new KullaniciOzetViewModel
            {
                Id = k.Id,
                TamAd = k.UnvanliAd,
                RolAdi = k.Unvan ?? string.Empty
            })
            .ToListAsync();
    }

    // ═══ ÖĞRENCİ ════════════════════════════════════════════════════════════
    public async Task<IEnumerable<OgrenciViewModel>> GetOgrencilerAsync()
    {
        return await _db.Ogrenciler
            .Include(o => o.Kullanici)
            .Include(o => o.Bolum)
            .OrderBy(o => o.OgrenciNo)
            .Select(o => new OgrenciViewModel
            {
                OgrenciId = o.Id,
                OgrenciNo = o.OgrenciNo,
                TamAd = o.Kullanici!.TamAd,
                Email = o.Kullanici.Email,
                BolumAdi = o.Bolum != null ? o.Bolum.BolumAdi : string.Empty,
                BolumKodu = o.Bolum != null ? o.Bolum.BolumKodu : string.Empty,
                Gano = o.Gano,
                DurumAdi = o.Durum.ToString()
            })
            .ToListAsync();
    }

    public async Task<ServiceResult> OgrenciDurumGuncelleAsync(int ogrenciId, OgrenciDurum yeniDurum, int userId)
    {
        var ogrenci = await _db.Ogrenciler.FindAsync(ogrenciId);
        if (ogrenci == null) return ServiceResult.Fail(AppConstants.ErrorMessages.KayitBulunamadi);

        var eskiDurum = ogrenci.Durum.ToString();
        ogrenci.Durum = yeniDurum;
        ogrenci.UpdatedAt = DateTime.UtcNow;
        ogrenci.UpdatedBy = userId;
        await _db.SaveChangesAsync();
        await _audit.WriteAsync(userId, AuditAction.Update, "Ogrenci", ogrenciId,
            eskiDurum, yeniDurum.ToString(), "Öğrenci durumu güncellendi");
        return ServiceResult.Ok();
    }

    // ═══ AUDİT LOG ══════════════════════════════════════════════════════════
    public async Task<AuditLogListViewModel> GetAuditLogsAsync(int page, int pageSize)
    {
        var query = _db.AuditLogs
            .Include(a => a.Kullanici)
            .OrderByDescending(a => a.Timestamp);

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AuditLogViewModel
            {
                Id = a.Id,
                KullaniciAdi = a.Kullanici != null ? a.Kullanici.TamAd : "Sistem",
                ActionAdi = a.Action.ToString(),
                Entity = a.EntityName ?? string.Empty,
                EntityId = a.EntityId,
                EskiDeger = a.OldValues,
                YeniDeger = a.NewValues,
                Aciklama = a.Details,
                Timestamp = a.Timestamp
            })
            .ToListAsync();

        return new AuditLogListViewModel
        {
            Logs = items,
            ToplamKayit = total,
            Sayfa = page,
            SayfaBoyutu = pageSize
        };
    }
}
