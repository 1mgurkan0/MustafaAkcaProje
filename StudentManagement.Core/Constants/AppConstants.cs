namespace StudentManagement.Core.Constants;

/// <summary>
/// UBYS — Uygulama genelinde kullanılan tüm sabitler.
/// </summary>
public static class AppConstants
{
    // ─── DataSeeder İçin Gerekli Kök Sabitler ───────────────────────
    public const string DefaultAdminUsername = "admin";
    public const string DefaultAdminPassword = "Admin123!!";
    public const string DefaultAdminEmail = "admin@ubys.com";
    public const int BcryptWorkFactor = 12;
    public const int PasswordMinLength = 6;

    // ─── Session Keys ───────────────────────────────────────────────
    public static class Session
    {
        public const string UserId = "UserId";
        public const string Username = "Username";
        public const string UserRole = "UserRole";
        public const string FullName = "FullName";
        public const string BolumId = "BolumId";
        public const string OgrenciId = "OgrenciId";
    }

    // ─── Rol Adları ─────────────────────────────────────────────────
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Ogretmen = "Ogretmen";
        public const string Ogrenci = "Ogrenci";
        public const string OgrenciIsleri = "OgrenciIsleri";
    }

    // ─── Route / URL Prefix ─────────────────────────────────────────
    public static class Routes
    {
        public const string AdminBase = "/Admin/Dashboard";
        public const string OgretmenBase = "/OgretmenPanel/Dashboard";
        public const string OgrenciBase = "/OgrenciPanel/Dashboard";
        public const string OgrenciIsleriBase = "/OgrenciIsleri/Dashboard";
        public const string Login = "/Auth/Login";
    }

    // ─── Seed Şifreleri ─────────────────────────────────────────────
    public static class SeedPasswords
    {
        public const string Admin = "Admin@12345";
        public const string Ogretmen = "Ogretmen@123";
        public const string Ogrenci = "Ogrenci@123";
        public const string OgrenciIsleri = "OgrIsleri@123";
    }

    // ─── Brute-Force / Güvenlik ─────────────────────────────────────
    public static class Security
    {
        public const int MaxLoginAttempts = 5;
        public const int LockoutMinutes = 15;
        public const int BcryptWorkFactor = 12;
        public const int SessionTimeoutMinutes = 30;
    }

    // ─── Not Hesaplama ──────────────────────────────────────────────
    public static class Grading
    {
        public const double VizeAgirlik = 0.40;
        public const double FinalAgirlik = 0.60;
        public const double GecmeEsigi = 45.0;

        // HarfNotu eşik değerleri (GenelNot ≥ X → HarfNotu)
        public const double AA_Esik = 90.0;
        public const double BA_Esik = 85.0;
        public const double BB_Esik = 80.0;
        public const double CB_Esik = 75.0;
        public const double CC_Esik = 70.0;
        public const double DC_Esik = 60.0;
        public const double DD_Esik = 45.0;
        // < 45 → FF
    }

    // ─── Not Katsayıları (4'lük sistem) ────────────────────────────
    public static class NotKatsayisi
    {
        public const double AA = 4.0;
        public const double BA = 3.5;
        public const double BB = 3.0;
        public const double CB = 2.5;
        public const double CC = 2.0;
        public const double DC = 1.5;
        public const double DD = 1.0;
        public const double FF = 0.0;
    }

    // ─── Sayfalama / Tablo ──────────────────────────────────────────
    public static class Pagination
    {
        public const int DefaultPageSize = 20;
        public const int DashboardTopCount = 5;
        public const int MaxPageSize = 100;
    }

    // ─── Dosya / Belge ──────────────────────────────────────────────
    public static class Belge
    {
        public const int MaxTalepPerOgrenci = 10;      // Aktif bekleyen talep limiti
        public const int IslemGunSuresi = 5;      // Belge hazırlama gün süresi
    }

    // ─── Ders / Kayıt ───────────────────────────────────────────────
    public static class DersKayit
    {
        public const int MinAkts = 1;
        public const int MaxAkts = 8;    // Tek ders max AKTS
        public const int MaxToplamAkts = 45; // Dönem max AKTS yükü
        public const int MinToplamAkts = 0;
    }

    // ─── Yoklama ────────────────────────────────────────────────────
    public static class Yoklama
    {
        public const double MinDevamYuzdesi = 70.0;   // %70 devam zorunlu
    }

    // ─── TempData Keys ──────────────────────────────────────────────
    public static class TempDataKeys
    {
        public const string SuccessMessage = "SuccessMessage";
        public const string ErrorMessage = "ErrorMessage";
        public const string WarningMessage = "WarningMessage";
        public const string InfoMessage = "InfoMessage";
    }

    // ─── Cache Keys ─────────────────────────────────────────────────
    public static class CacheKeys
    {
        public const string AktifDonem = "AktifDonem";
        public const string BolumListesi = "BolumListesi";
        public const string DersKatalogu = "DersKatalogu";
    }

    // ─── UI Renk Temaları (rol bazlı) ───────────────────────────────
    public static class RolRenk
    {
        public const string Admin = "#ef4444";   // Kırmızı
        public const string Ogretmen = "#10b981";   // Yeşil
        public const string OgrenciIsleri = "#06b6d4";   // Cyan
        public const string Ogrenci = "#4f46e5";   // Mor
    }

    // ─── Bölüm Kodları ──────────────────────────────────────────────
    public static class BolumKodlari
    {
        public const string Bilgisayar = "BIL";
        public const string YazilimMuhendisligi = "YMH";
        public const string ElektrikElektronik = "EEE";
        public const string EndüstriMuhendisligi = "ENM";
        public const string Matematik = "MAT";
    }

    // ─── Hata Mesajları ─────────────────────────────────────────────
    public static class ErrorMessages
    {
        public const string KullaniciBulunamadi = "Kullanıcı bulunamadı.";
        public const string HataliKullaniciAdi = "Kullanıcı adı veya şifre hatalı.";
        public const string HesapKilitli = "Hesabınız {0} dakika kilitlenmiştir.";
        public const string YetkiYok = "Bu işlem için yetkiniz bulunmamaktadır.";
        public const string KayitBulunamadi = "Kayıt bulunamadı.";
        public const string KontenjanDolu = "Dersin kontenjanı dolmuştur.";
        public const string ZatenKayitli = "Bu derse zaten kayıtlısınız.";
        public const string DersKayitKapali = "Ders kayıt dönemi aktif değil.";
        public const string AktifDonemYok = "Sistemde aktif dönem bulunmamaktadır.";
        public const string GenelHata = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
        public const string DogrulamaHatasi = "Form bilgileri geçersiz.";
    }

    // ─── Başarı Mesajları ────────────────────────────────────────────
    public static class SuccessMessages
    {
        public const string KayitOlusturuldu = "Kayıt başarıyla oluşturuldu.";
        public const string KayitGuncellendi = "Kayıt başarıyla güncellendi.";
        public const string KayitSilindi = "Kayıt başarıyla silindi.";
        public const string DersKayitOlusturuldu = "Ders kayıt talebiniz iletildi.";
        public const string DersKayitOnaylandi = "Ders kaydı onaylandı.";
        public const string DersKayitReddedildi = "Ders kaydı reddedildi.";
        public const string NotKaydedildi = "Notlar başarıyla kaydedildi.";
        public const string YoklamaKaydedildi = "Yoklama kaydedildi.";
        public const string BelgeTalebiOlusturuldu = "Belge talebiniz oluşturuldu.";
        public const string DuyuruYayinlandi = "Duyuru başarıyla yayınlandı.";
        public const string SifreGuncellendi = "Şifre başarıyla güncellendi.";
    }
}