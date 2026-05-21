using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentManagement.Core.Constants;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;
using StudentManagement.Data.Context;
using StudentManagement.Services.Interfaces;
using StudentManagement.Services.ViewModels.Admin;
using StudentManagement.Services.ViewModels.Auth;

namespace StudentManagement.Services.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    private readonly IAuditLogWriter _auditLog;
    private readonly ILogger<AuthService> _logger;

    // Brute-force: IP → (sayaç, son deneme)
    private static readonly Dictionary<string, (int Count, DateTime LastAttempt)> _loginAttempts = new();
    private static readonly object _lock = new();

    public AuthService(
        ApplicationDbContext db,
        IMapper mapper,
        IAuditLogWriter auditLog,
        ILogger<AuthService> logger)
    {
        _db       = db;
        _mapper   = mapper;
        _auditLog = auditLog;
        _logger   = logger;
    }

    // ─── LOGIN ────────────────────────────────────────────────────────────────
    public async Task<ServiceResult<LoginSessionData>> LoginAsync(LoginViewModel model, string? ipAddress)
    {
        var ip = ipAddress ?? "unknown";

        // Brute-force kontrolü
        lock (_lock)
        {
            if (_loginAttempts.TryGetValue(ip, out var att))
            {
                var lockoutEnd = att.LastAttempt.AddMinutes(AppConstants.Security.LockoutMinutes);
                if (att.Count >= AppConstants.Security.MaxLoginAttempts && DateTime.UtcNow < lockoutEnd)
                {
                    var remaining = (int)(lockoutEnd - DateTime.UtcNow).TotalMinutes + 1;
                    return ServiceResult<LoginSessionData>.Fail(
                        string.Format(AppConstants.ErrorMessages.HesapKilitli, remaining));
                }

                // Kilidi süresi geçtiyse sıfırla
                if (DateTime.UtcNow >= lockoutEnd)
                    _loginAttempts.Remove(ip);
            }
        }

        // Kullanıcıyı bul
        var kullanici = await _db.Kullanicilar
            .Include(k => k.Ogrenci)
            .FirstOrDefaultAsync(k => k.KullaniciAdi == model.KullaniciAdi && k.IsActive);

        if (kullanici == null || !BCrypt.Net.BCrypt.Verify(model.Sifre, kullanici.SifreHash))
        {
            // Başarısız denemeyi kaydet
            lock (_lock)
            {
                if (_loginAttempts.TryGetValue(ip, out var att))
                    _loginAttempts[ip] = (att.Count + 1, DateTime.UtcNow);
                else
                    _loginAttempts[ip] = (1, DateTime.UtcNow);
            }

            await _auditLog.WriteAsync(
                userId: 0,
                action: AuditAction.Login,
                entity: "Kullanici",
                aciklama: $"Başarısız giriş: {model.KullaniciAdi} [{ip}]");

            return ServiceResult<LoginSessionData>.Fail(AppConstants.ErrorMessages.HataliKullaniciAdi);
        }

        // Başarılı → sayacı sıfırla
        lock (_lock) { _loginAttempts.Remove(ip); }

        await _auditLog.WriteAsync(kullanici.Id, AuditAction.Login, "Kullanici", kullanici.Id,
            aciklama: $"Giriş [{ip}]");

        var data = new LoginSessionData
        {
            UserId   = kullanici.Id,
            Username = kullanici.KullaniciAdi,
            FullName = kullanici.TamAd,
            Role     = kullanici.Rol.ToString(),
            OgrenciId = kullanici.Ogrenci?.Id,
            BolumId   = kullanici.Ogrenci?.BolumId
        };

        return ServiceResult<LoginSessionData>.Ok(data);
    }

    // ─── REGISTER ─────────────────────────────────────────────────────────────
    public async Task<ServiceResult> RegisterAsync(RegisterViewModel model)
    {
        // Kullanıcı adı unique kontrolü
        var mevcutKullanici = await _db.Kullanicilar
            .AnyAsync(k => k.KullaniciAdi == model.KullaniciAdi);

        if (mevcutKullanici)
            return ServiceResult.Fail("Bu kullanıcı adı zaten kullanılıyor.");

        // Email unique kontrolü
        var mevcutEmail = await _db.Kullanicilar
            .AnyAsync(k => k.Email == model.Email);

        if (mevcutEmail)
            return ServiceResult.Fail("Bu e-posta adresi zaten kayıtlı.");

        // Bölüm kontrol
        var bolum = await _db.Bolumler.FindAsync(model.BolumId);
        if (bolum == null)
            return ServiceResult.Fail("Seçilen bölüm bulunamadı.");

        await using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            // Kullanıcı oluştur
            var kullanici = new Kullanici
            {
                KullaniciAdi = model.KullaniciAdi,
                Ad           = model.Ad,
                Soyad        = model.Soyad,
                Email        = model.Email,
                SifreHash    = BCrypt.Net.BCrypt.HashPassword(model.Sifre, AppConstants.Security.BcryptWorkFactor),
                Rol          = KullaniciRol.Ogrenci,   // Register = Ogrenci hardcoded
                IsActive     = true,
                CreatedAt    = DateTime.UtcNow,
                UpdatedAt    = DateTime.UtcNow
            };

            _db.Kullanicilar.Add(kullanici);
            await _db.SaveChangesAsync();

            // Öğrenci numarası üret: yılSON4 + ID
            var ogrenciNo = $"{DateTime.UtcNow.Year}{kullanici.Id:D4}";

            var ogrenci = new Ogrenci
            {
                KullaniciId   = kullanici.Id,
                BolumId       = model.BolumId,
                OgrenciNo     = ogrenciNo,
                Durum         = OgrenciDurum.Aktif,
                Gano          = 0,
                TamamlananAkts = 0,
                IsActive      = true,
                CreatedAt     = DateTime.UtcNow,
                UpdatedAt     = DateTime.UtcNow
            };

            _db.Ogrenciler.Add(ogrenci);
            await _db.SaveChangesAsync();

            await _auditLog.WriteAsync(kullanici.Id, AuditAction.Create, "Ogrenci", ogrenci.Id,
                aciklama: $"Yeni öğrenci kaydı: {kullanici.TamAd}");

            await transaction.CommitAsync();
            _logger.LogInformation("Yeni öğrenci kaydedildi: {Username}", model.KullaniciAdi);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Register hatası: {Username}", model.KullaniciAdi);
            return ServiceResult.Fail(AppConstants.ErrorMessages.GenelHata);
        }
    }

    // ─── BÖLÜM LİSTESİ ───────────────────────────────────────────────────────
    public async Task<IEnumerable<BolumSelectViewModel>> GetBolumlerAsync()
    {
        return await _db.Bolumler
            .Where(b => b.IsActive)
            .OrderBy(b => b.BolumAdi)
            .Select(b => new BolumSelectViewModel
            {
                Id          = b.Id,
                DisplayText = $"{b.BolumKodu} — {b.BolumAdi}"
            })
            .ToListAsync();
    }
}
