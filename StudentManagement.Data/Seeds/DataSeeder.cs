using Microsoft.EntityFrameworkCore;
using StudentManagement.Core.Constants;
using StudentManagement.Core.Entities;
using StudentManagement.Core.Enums;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.Seeds;

public static class DataSeeder
{
    private static readonly Random _rng = new(42);

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Kullanicilar.IgnoreQueryFilters().AnyAsync()) return;

        // ═════════════════════════════════════════════════════════════════════
        // 1. BÖLÜMLER
        // ═════════════════════════════════════════════════════════════════════
        var bolumler = new List<Bolum>
        {
            new() { BolumKodu = "BIL", BolumAdi = "Bilgisayar Mühendisliği",          Fakulte = "Mühendislik Fakültesi",  MinMezuniyetAkts = 240, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { BolumKodu = "YMH", BolumAdi = "Yazılım Mühendisliği",             Fakulte = "Mühendislik Fakültesi",  MinMezuniyetAkts = 240, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { BolumKodu = "EEE", BolumAdi = "Elektrik-Elektronik Mühendisliği", Fakulte = "Mühendislik Fakültesi",  MinMezuniyetAkts = 240, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { BolumKodu = "ENM", BolumAdi = "Endüstri Mühendisliği",            Fakulte = "Mühendislik Fakültesi",  MinMezuniyetAkts = 240, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { BolumKodu = "MAT", BolumAdi = "Matematik",                         Fakulte = "Fen-Edebiyat Fakültesi", MinMezuniyetAkts = 240, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        };

        await context.Bolumler.AddRangeAsync(bolumler);
        await context.SaveChangesAsync();

        var bilBolum = bolumler[0];
        var ymhBolum = bolumler[1];
        var eeeBolum = bolumler[2];
        var enmBolum = bolumler[3];
        var matBolum = bolumler[4];

        // ═════════════════════════════════════════════════════════════════════
        // 2. DÖNEMLER
        // ═════════════════════════════════════════════════════════════════════
        var donemler = new List<Donem>
        {
            new() {
                DonemKodu = "20231", DonemAdi = "2023-2024 Güz Dönemi",
                Yil = 2023, DonemTur = DonemTur.Guz, AktifMi = false,
                BaslangicTarihi = new DateTime(2023, 9, 18),  BitisTarihi = new DateTime(2024, 1, 19),
                DersKayitBaslangic = new DateTime(2023, 9, 1), DersKayitBitis = new DateTime(2023, 9, 17),
                IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            },
            new() {
                DonemKodu = "20232", DonemAdi = "2023-2024 Bahar Dönemi",
                Yil = 2023, DonemTur = DonemTur.Bahar, AktifMi = false,
                BaslangicTarihi = new DateTime(2024, 2, 19),  BitisTarihi = new DateTime(2024, 6, 14),
                DersKayitBaslangic = new DateTime(2024, 2, 1), DersKayitBitis = new DateTime(2024, 2, 18),
                IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            },
            new() {
                DonemKodu = "20241", DonemAdi = "2024-2025 Güz Dönemi",
                Yil = 2024, DonemTur = DonemTur.Guz, AktifMi = true,   // ← AKTİF DÖNEM
                BaslangicTarihi = new DateTime(2024, 9, 16),  BitisTarihi = new DateTime(2025, 1, 17),
                DersKayitBaslangic = new DateTime(2024, 9, 1), DersKayitBitis = new DateTime(2025, 1, 31),
                IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            },
        };

        await context.Donemler.AddRangeAsync(donemler);
        await context.SaveChangesAsync();

        var aktifDonem = donemler[2];
        var oncekiDonem1 = donemler[1];
        var oncekiDonem2 = donemler[0];

        // ═════════════════════════════════════════════════════════════════════
        // 3. KULLANICILAR — Admin, Öğrenci İşleri, Öğretmenler
        // ═════════════════════════════════════════════════════════════════════
        var adminKullanici = new Kullanici
        {
            KullaniciAdi = AppConstants.DefaultAdminUsername,
            SifreHash = BCrypt.Net.BCrypt.HashPassword(AppConstants.DefaultAdminPassword, AppConstants.BcryptWorkFactor),
            Ad = "Sistem",
            Soyad = "Yöneticisi",
            Email = AppConstants.DefaultAdminEmail,
            Rol = KullaniciRol.Admin,
            IsActive = true,
            LastPasswordChangeDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await context.Kullanicilar.AddAsync(adminKullanici);

        // Öğrenci İşleri personeli
        var ogrenciIsleriPersonel = new[]
        {
            new Kullanici { KullaniciAdi = "ogr.isleri1",  SifreHash = BCrypt.Net.BCrypt.HashPassword("OgrIsleri@123", AppConstants.BcryptWorkFactor), Ad = "Aylin",   Soyad = "Çelik",   Email = "aylin.celik@universite.edu.tr",   Telefon = "03121234001", Rol = KullaniciRol.OgrenciIsleri, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Kullanici { KullaniciAdi = "ogr.isleri2",  SifreHash = BCrypt.Net.BCrypt.HashPassword("OgrIsleri@123", AppConstants.BcryptWorkFactor), Ad = "Bülent",  Soyad = "Aksoy",   Email = "bulent.aksoy@universite.edu.tr",   Telefon = "03121234002", Rol = KullaniciRol.OgrenciIsleri, IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        };
        await context.Kullanicilar.AddRangeAsync(ogrenciIsleriPersonel);

        // Öğretmenler — unvan + bölüm atamalı
        var ogretmenVerileri = new (string Ad, string Soyad, string KulAdi, string Email, string Tel, string Unvan, Bolum Bolum)[]
        {
            ("Ahmet",   "Yılmaz",  "ahmet.yilmaz",   "ahmet.yilmaz@universite.edu.tr",   "05321234001", "Prof.Dr.",         bilBolum),
            ("Fatma",   "Kaya",    "fatma.kaya",      "fatma.kaya@universite.edu.tr",     "05321234002", "Doç.Dr.",          bilBolum),
            ("Mehmet",  "Demir",   "mehmet.demir",    "mehmet.demir@universite.edu.tr",   "05321234003", "Dr.Öğr.Üyesi",     bilBolum),
            ("Ayşe",    "Çelik",   "ayse.celik",      "ayse.celik@universite.edu.tr",     "05321234004", "Dr.Öğr.Üyesi",     bilBolum),
            ("Mustafa", "Şahin",   "mustafa.sahin",   "mustafa.sahin@universite.edu.tr",  "05321234005", "Prof.Dr.",         ymhBolum),
            ("Zeynep",  "Arslan",  "zeynep.arslan",   "zeynep.arslan@universite.edu.tr",  "05321234006", "Doç.Dr.",          ymhBolum),
            ("Hüseyin", "Doğan",   "huseyin.dogan",   "huseyin.dogan@universite.edu.tr",  "05321234007", "Arş.Gör.Dr.",      ymhBolum),
            ("Elif",    "Yıldız",  "elif.yildiz",     "elif.yildiz@universite.edu.tr",    "05321234008", "Dr.Öğr.Üyesi",     eeeBolum),
            ("İbrahim", "Koç",     "ibrahim.koc",     "ibrahim.koc@universite.edu.tr",    "05321234009", "Prof.Dr.",         eeeBolum),
            ("Hatice",  "Kurt",    "hatice.kurt",     "hatice.kurt@universite.edu.tr",    "05321234010", "Doç.Dr.",          enmBolum),
            ("Ali",     "Özdemir", "ali.ozdemir",     "ali.ozdemir@universite.edu.tr",    "05321234011", "Dr.Öğr.Üyesi",     enmBolum),
            ("Merve",   "Aydın",   "merve.aydin",     "merve.aydin@universite.edu.tr",    "05321234012", "Arş.Gör.",         bilBolum),
            ("Emre",    "Erdoğan", "emre.erdogan",    "emre.erdogan@universite.edu.tr",   "05321234013", "Dr.Öğr.Üyesi",     bilBolum),
            ("Selin",   "Güneş",   "selin.gunes",     "selin.gunes@universite.edu.tr",    "05321234014", "Prof.Dr.",         matBolum),
            ("Burak",   "Polat",   "burak.polat",     "burak.polat@universite.edu.tr",    "05321234015", "Doç.Dr.",          matBolum),
        };

        var ogretmenEntities = ogretmenVerileri.Select(o => new Kullanici
        {
            KullaniciAdi = o.KulAdi,
            SifreHash = BCrypt.Net.BCrypt.HashPassword("Ogretmen@123", AppConstants.BcryptWorkFactor),
            Ad = o.Ad,
            Soyad = o.Soyad,
            Email = o.Email,
            Telefon = o.Tel,
            Unvan = o.Unvan,
            Bolum = o.Bolum,
            Rol = KullaniciRol.Ogretmen,
            IsActive = true,
            LastPasswordChangeDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }).ToList();

        await context.Kullanicilar.AddRangeAsync(ogretmenEntities);
        await context.SaveChangesAsync();

        // ═════════════════════════════════════════════════════════════════════
        // 4. DERSLER (ders kataloğu — dönem/hoca bağımsız)
        // ═════════════════════════════════════════════════════════════════════
        var dersVerileri = new (string Kod, string Ad, int Kredi, int Akts, int TeoriSaat, int UygSaat, int MaxKont, Bolum Bolum, string Aciklama)[]
        {
            ("BIL101", "Algoritma ve Programlamaya Giriş",    3, 5, 3, 0, 40, bilBolum, "Temel algoritmik düşünce ve programlama."),
            ("BIL102", "Nesne Yönelimli Programlama",         3, 6, 2, 2, 35, bilBolum, "OOP prensipleri ve C# ile uygulama."),
            ("BIL103", "Veri Yapıları ve Algoritmalar",       3, 6, 2, 2, 35, bilBolum, "Liste, ağaç, graf veri yapıları."),
            ("BIL104", "Bilgisayar Mimarisi",                 3, 5, 3, 0, 40, bilBolum, "İşlemci mimarisi ve bellek yönetimi."),
            ("BIL201", "Veritabanı Yönetim Sistemleri",       3, 6, 2, 2, 35, bilBolum, "İlişkisel veritabanı tasarımı ve SQL."),
            ("BIL202", "Bilgisayar Ağları",                   3, 6, 3, 0, 40, bilBolum, "OSI modeli, TCP/IP protokolleri."),
            ("BIL203", "Web Programlama",                     3, 5, 2, 2, 35, bilBolum, "HTML, CSS, JS ve backend geliştirme."),
            ("BIL301", "Yazılım Mühendisliği",                3, 6, 3, 0, 40, bilBolum, "Yazılım geliştirme süreçleri ve metodolojiler."),
            ("BIL302", "İşletim Sistemleri",                  3, 6, 3, 0, 40, bilBolum, "Süreç yönetimi, bellek ve dosya sistemi."),
            ("BIL401", "Makine Öğrenmesi",                    3, 7, 2, 2, 30, bilBolum, "Denetimli ve denetimsiz öğrenme yöntemleri."),
            ("YMH101", "Programlamaya Giriş",                 3, 5, 2, 2, 40, ymhBolum, "Python ile programlamaya giriş."),
            ("YMH201", "İleri Yazılım Geliştirme",            3, 6, 2, 2, 35, ymhBolum, "Design patterns ve clean code."),
            ("YMH301", "Mobil Uygulama Geliştirme",           3, 6, 2, 2, 30, ymhBolum, "Android ve iOS uygulama geliştirme."),
            ("EEE101", "Devre Analizi",                       4, 6, 3, 2, 35, eeeBolum, "Temel devre teorisi ve analiz yöntemleri."),
            ("EEE201", "Elektronik",                          4, 6, 3, 2, 35, eeeBolum, "Yarı iletken cihazlar ve devreleri."),
            ("ENM101", "Yöneylem Araştırması",                3, 5, 3, 0, 40, enmBolum, "Doğrusal programlama ve optimizasyon."),
            ("ENM201", "Üretim Yönetimi",                     3, 5, 3, 0, 40, enmBolum, "Üretim planlama ve çizelgeleme."),
            ("MAT101", "Matematik I",                         4, 6, 4, 0, 50, matBolum, "Fonksiyonlar, türev ve integral."),
            ("MAT102", "Matematik II",                        4, 6, 4, 0, 50, matBolum, "Çok değişkenli hesap ve diferansiyel denklemler."),
            ("MAT201", "Olasılık ve İstatistik",              3, 5, 3, 0, 45, matBolum, "Olasılık teorisi ve istatistiksel analiz."),
            ("MAT202", "Lineer Cebir",                        3, 5, 3, 0, 45, matBolum, "Matrisler, vektörler ve lineer dönüşümler."),
            ("ING101", "Mesleki İngilizce I",                 2, 3, 2, 0, 50, bilBolum, "Akademik okuma ve yazma becerileri."),
            ("TUR101", "Türk Dili ve Edebiyatı I",            2, 2, 2, 0, 60, bilBolum, "Türk dili bilgisi ve yazılı anlatım."),
            ("ATA101", "Atatürk İlkeleri ve İnkılap Tarihi",  2, 2, 2, 0, 60, bilBolum, "Cumhuriyet tarihi ve Atatürk ilkeleri."),
            ("BIL105", "Siber Güvenlik Temelleri",            3, 5, 3, 0, 35, bilBolum, "Ağ güvenliği ve kriptografi temelleri."),
        };

        var dersler = dersVerileri.Select(d => new Ders
        {
            DersKodu = d.Kod,
            DersAdi = d.Ad,
            Kredi = d.Kredi,
            Akts = d.Akts,
            TeoriSaat = d.TeoriSaat,
            UygulamaSaat = d.UygSaat,
            MaxKontenjan = d.MaxKont,
            Bolum = d.Bolum,
            Aciklama = d.Aciklama,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }).ToList();

        await context.Dersler.AddRangeAsync(dersler);
        await context.SaveChangesAsync();

        // ═════════════════════════════════════════════════════════════════════
        // 5. DERS ATAMALARI (Öğretmen + Ders + Dönem + Gün/Saat)
        // ═════════════════════════════════════════════════════════════════════
        var gun = DersGun.Pazartesi;
        var gunler = Enum.GetValues<DersGun>();
        var dersAtamalar = new List<DersAtama>();

        // Yardımcı: ders kodu → entity
        Ders D(string kod) => dersler.First(d => d.DersKodu == kod);
        Kullanici O(string kulAdi) => ogretmenEntities.First(k => k.KullaniciAdi == kulAdi);

        // ── Aktif dönem atamaları ────────────────────────────────────────────
        var aktifAtamalar = new (Ders Ders, Kullanici Ogretmen, DersGun Gun, string Saat, string Derslik)[]
        {
            (D("BIL101"), O("ahmet.yilmaz"),  DersGun.Pazartesi, "09:00", "A-101"),
            (D("BIL102"), O("fatma.kaya"),    DersGun.Sali,      "10:00", "A-102"),
            (D("BIL103"), O("mehmet.demir"),  DersGun.Carsamba,  "09:00", "Lab-1"),
            (D("BIL104"), O("ayse.celik"),    DersGun.Persembe,  "11:00", "A-103"),
            (D("BIL201"), O("fatma.kaya"),    DersGun.Cuma,      "09:00", "Lab-2"),
            (D("BIL202"), O("emre.erdogan"),  DersGun.Pazartesi, "11:00", "A-201"),
            (D("BIL203"), O("merve.aydin"),   DersGun.Sali,      "14:00", "Lab-3"),
            (D("BIL301"), O("mehmet.demir"),  DersGun.Carsamba,  "13:00", "A-202"),
            (D("BIL302"), O("ahmet.yilmaz"),  DersGun.Persembe,  "09:00", "A-203"),
            (D("BIL401"), O("emre.erdogan"),  DersGun.Cuma,      "13:00", "Lab-1"),
            (D("YMH101"), O("mustafa.sahin"), DersGun.Pazartesi, "13:00", "B-101"),
            (D("YMH201"), O("zeynep.arslan"), DersGun.Sali,      "09:00", "B-102"),
            (D("YMH301"), O("huseyin.dogan"), DersGun.Carsamba,  "11:00", "Lab-4"),
            (D("EEE101"), O("elif.yildiz"),   DersGun.Persembe,  "13:00", "C-101"),
            (D("EEE201"), O("ibrahim.koc"),   DersGun.Cuma,      "11:00", "Lab-5"),
            (D("ENM101"), O("hatice.kurt"),   DersGun.Pazartesi, "09:00", "D-101"),
            (D("ENM201"), O("ali.ozdemir"),   DersGun.Sali,      "13:00", "D-102"),
            (D("MAT101"), O("selin.gunes"),   DersGun.Carsamba,  "09:00", "Amfi-1"),
            (D("MAT102"), O("burak.polat"),   DersGun.Persembe,  "09:00", "Amfi-2"),
            (D("MAT201"), O("selin.gunes"),   DersGun.Cuma,      "09:00", "A-104"),
            (D("MAT202"), O("burak.polat"),   DersGun.Pazartesi, "14:00", "A-105"),
            (D("ING101"), O("ahmet.yilmaz"),  DersGun.Sali,      "11:00", "B-201"),
            (D("TUR101"), O("fatma.kaya"),    DersGun.Carsamba,  "14:00", "Amfi-3"),
            (D("ATA101"), O("mehmet.demir"),  DersGun.Persembe,  "14:00", "Amfi-3"),
            (D("BIL105"), O("emre.erdogan"),  DersGun.Cuma,      "14:00", "Lab-2"),
        };

        foreach (var a in aktifAtamalar)
        {
            var saatParts = a.Saat.Split(':');
            var basSaat = new TimeSpan(int.Parse(saatParts[0]), int.Parse(saatParts[1]), 0);
            dersAtamalar.Add(new DersAtama
            {
                Ders = a.Ders,
                Donem = aktifDonem,
                Ogretmen = a.Ogretmen,
                Gun = a.Gun,
                BaslangicSaati = basSaat,
                BitisSaati = basSaat.Add(TimeSpan.FromHours(2)),
                Derslik = a.Derslik,
                KayitliOgrenciSayisi = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        // ── Önceki dönem atamaları (sadece BIL + MAT dersleri — geçmiş) ─────
        var eskiAtamalar = new (Ders Ders, Kullanici Ogretmen)[]
        {
            (D("BIL101"), O("ahmet.yilmaz")), (D("BIL102"), O("fatma.kaya")),
            (D("BIL103"), O("mehmet.demir")), (D("MAT101"), O("selin.gunes")),
            (D("MAT102"), O("burak.polat")),  (D("ING101"), O("ahmet.yilmaz")),
            (D("TUR101"), O("fatma.kaya")),   (D("ATA101"), O("mehmet.demir")),
        };

        foreach (var a in eskiAtamalar)
        {
            dersAtamalar.Add(new DersAtama
            {
                Ders = a.Ders,
                Donem = oncekiDonem1,
                Ogretmen = a.Ogretmen,
                Gun = gunler[_rng.Next(gunler.Length)],
                BaslangicSaati = new TimeSpan(9, 0, 0),
                BitisSaati = new TimeSpan(11, 0, 0),
                Derslik = "A-101",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        await context.DersAtamalar.AddRangeAsync(dersAtamalar);
        await context.SaveChangesAsync();

        // ═════════════════════════════════════════════════════════════════════
        // 6. ÖĞRENCİLER
        // ═════════════════════════════════════════════════════════════════════
        var erkekAdlar = new[] { "Ahmet", "Mehmet", "Mustafa", "Ali", "Hüseyin", "İbrahim", "Emre", "Burak", "Enes", "Furkan", "Kerem", "Serkan", "Tolga", "Uğur", "Volkan", "Kaan", "Mert", "Onur", "Berk", "Can", "Cem", "Deniz", "Ercan", "Fatih", "Gökhan", "Haluk", "İlhan", "Kadir", "Levent", "Murat" };
        var kizAdlar = new[] { "Ayşe", "Fatma", "Zeynep", "Elif", "Merve", "Selin", "Hatice", "Büşra", "Esra", "Gamze", "Hande", "İrem", "Kübra", "Leyla", "Meltem", "Nur", "Özge", "Pınar", "Rabia", "Sibel", "Tuğba", "Ülkü", "Vildan", "Yasemin", "Zühal", "Arzu", "Burcu", "Cansu", "Derya", "Ebru" };
        var soyadlar = new[] { "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Arslan", "Doğan", "Yıldız", "Koç", "Kurt", "Özdemir", "Aydın", "Erdoğan", "Güneş", "Polat", "Çetin", "Bulut", "Çiftçi", "Çalışkan", "Keskin", "Güler", "Aktaş", "Karahan", "Akın", "Kılıç", "Tunç", "Karakuş", "Duman", "Yalçın", "Şimşek" };

        // Bölüm dağılımı: BIL 70, YMH 50, EEE 30, ENM 30, MAT 20
        var bolumDagilimi = new (Bolum Bolum, int Adet)[]
        {
            (bilBolum, 70), (ymhBolum, 50), (eeeBolum, 30), (enmBolum, 30), (matBolum, 20)
        };

        var ogrenciKullanicilar = new List<Kullanici>();
        var ogrenciler = new List<Ogrenci>();
        int sayac = 1;

        foreach (var (bolum, adet) in bolumDagilimi)
        {
            for (int i = 0; i < adet; i++)
            {
                bool erkek = _rng.Next(2) == 0;
                string ad = erkek ? erkekAdlar[_rng.Next(erkekAdlar.Length)] : kizAdlar[_rng.Next(kizAdlar.Length)];
                string soyad = soyadlar[_rng.Next(soyadlar.Length)];
                string kulAdi = $"{Temizle(ad)}.{Temizle(soyad)}{sayac:D3}".ToLower();
                string ogrNo = $"2024{sayac:D5}";
                var dogum = new DateTime(_rng.Next(1998, 2006), _rng.Next(1, 13), _rng.Next(1, 28));

                var kul = new Kullanici
                {
                    KullaniciAdi = kulAdi,
                    SifreHash = BCrypt.Net.BCrypt.HashPassword("Ogrenci@123", AppConstants.BcryptWorkFactor),
                    Ad = ad,
                    Soyad = soyad,
                    Email = $"{kulAdi}@ogrenci.edu.tr",
                    Telefon = $"053{_rng.Next(10000000, 99999999)}",
                    Rol = KullaniciRol.Ogrenci,
                    IsActive = true,
                    LastPasswordChangeDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var ogr = new Ogrenci
                {
                    OgrenciNo = ogrNo,
                    Bolum = bolum,
                    AktifDonem = aktifDonem,
                    SinifSeviyesi = _rng.Next(1, 5),
                    Durum = OgrenciDurum.Aktif,
                    Cinsiyet = erkek ? "Erkek" : "Kadın",
                    DogumTarihi = dogum,
                    KayitTarihi = DateTime.UtcNow.AddDays(-_rng.Next(30, 730)),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Kullanici = kul
                };

                ogrenciKullanicilar.Add(kul);
                ogrenciler.Add(ogr);
                sayac++;
            }
        }

        await context.Kullanicilar.AddRangeAsync(ogrenciKullanicilar);
        await context.SaveChangesAsync();
        await context.Ogrenciler.AddRangeAsync(ogrenciler);
        await context.SaveChangesAsync();

        // ═════════════════════════════════════════════════════════════════════
        // 7. DERS KAYITLARI — Aktif dönem (Devam) + Önceki dönem (Tamamlandi)
        // ═════════════════════════════════════════════════════════════════════
        var aktifAtamaEntities = dersAtamalar.Where(da => da.DonemId == aktifDonem.Id).ToList();
        var eskiAtamaEntities = dersAtamalar.Where(da => da.DonemId == oncekiDonem1.Id).ToList();
        var ogrenciListesi = await context.Ogrenciler.Include(o => o.Bolum).ToListAsync();
        var kayitlar = new List<OgrenciDers>();

        foreach (var ogr in ogrenciListesi)
        {
            // Öğrencinin bölümüne ait aktif dönem atamaları
            var uygunAtamalar = aktifAtamaEntities
                .Where(da => da.Ders.BolumId == ogr.BolumId
                          || da.Ders.DersKodu.StartsWith("ING")
                          || da.Ders.DersKodu.StartsWith("TUR")
                          || da.Ders.DersKodu.StartsWith("ATA"))
                .ToList();

            int dersSayisi = _rng.Next(4, 7);
            var secilenler = uygunAtamalar.OrderBy(_ => _rng.Next()).Take(dersSayisi);

            foreach (var atama in secilenler)
            {
                kayitlar.Add(new OgrenciDers
                {
                    Ogrenci = ogr,
                    DersAtama = atama,
                    Durum = OgrenciDersDurum.Devam,
                    KayitTarihi = aktifDonem.BaslangicTarihi.AddDays(_rng.Next(0, 5)),
                    OnayTarihi = aktifDonem.BaslangicTarihi.AddDays(-2),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                atama.KayitliOgrenciSayisi++;
            }

            // Önceki dönem kayıtları — tamamlanmış, notlu
            var eskiUygunlar = eskiAtamaEntities
                .Where(da => da.Ders.BolumId == ogr.BolumId
                          || da.Ders.DersKodu.StartsWith("ING")
                          || da.Ders.DersKodu.StartsWith("TUR")
                          || da.Ders.DersKodu.StartsWith("ATA"))
                .ToList();

            int eskiDersSayisi = _rng.Next(3, 6);
            var eskiSecilenler = eskiUygunlar.OrderBy(_ => _rng.Next()).Take(eskiDersSayisi);

            foreach (var atama in eskiSecilenler)
            {
                decimal vize = Math.Round((decimal)(_rng.Next(40, 95) + _rng.NextDouble()), 1);
                decimal final = Math.Round((decimal)(_rng.Next(40, 95) + _rng.NextDouble()), 1);
                decimal genel = Math.Round(vize * 0.4m + final * 0.6m, 1);
                var harf = HesaplaHarfNotu(genel);
                decimal kats = HesaplaKatsayi(harf);

                kayitlar.Add(new OgrenciDers
                {
                    Ogrenci = ogr,
                    DersAtama = atama,
                    Durum = OgrenciDersDurum.Tamamlandi,
                    VizeNotu = vize,
                    FinalNotu = final,
                    GenelNot = genel,
                    HarfNotu = harf,
                    NotKatsayisi = kats,
                    GectiMi = genel >= 45,
                    KayitTarihi = oncekiDonem1.BaslangicTarihi.AddDays(_rng.Next(0, 5)),
                    OnayTarihi = oncekiDonem1.BaslangicTarihi.AddDays(-2),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }

        await context.OgrenciDersler.AddRangeAsync(kayitlar);
        await context.SaveChangesAsync();

        // GANO hesapla ve güncelle
        foreach (var ogr in ogrenciListesi)
        {
            var bitmisDersler = kayitlar
                .Where(k => k.OgrenciId == ogr.Id
                         && k.Durum == OgrenciDersDurum.Tamamlandi
                         && k.NotKatsayisi.HasValue)
                .ToList();

            if (bitmisDersler.Any())
            {
                int toplamAkts = bitmisDersler.Sum(k => k.DersAtama.Ders.Akts);
                decimal agirlikli = bitmisDersler.Sum(k => k.NotKatsayisi!.Value * k.DersAtama.Ders.Akts);
                ogr.Gano = toplamAkts > 0 ? Math.Round(agirlikli / toplamAkts, 2) : null;
                ogr.TamamlananAkts = bitmisDersler
                    .Where(k => k.GectiMi == true)
                    .Sum(k => k.DersAtama.Ders.Akts);
            }
        }

        await context.SaveChangesAsync();

        // ═════════════════════════════════════════════════════════════════════
        // 8. DUYURULAR
        // ═════════════════════════════════════════════════════════════════════
        var duyurular = new List<Duyuru>
        {
            new() {
                Baslik = "2024-2025 Güz Dönemi Ders Kayıtları Hakkında",
                Icerik = "Güz dönemi ders kayıtları başlamıştır. Danışmanınız ile görüşerek ders seçimlerinizi tamamlayınız.",
                Hedef = DuyuruHedef.Ogrenciler, Onemli = true, YayinlayanId = adminKullanici.Id,
                YayinTarihi = DateTime.UtcNow.AddDays(-10),
                IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            },
            new() {
                Baslik = "Dönem Sonu Sınav Takvimi Yayınlandı",
                Icerik = "2024-2025 Güz Dönemi final sınav takvimi öğrenci bilgi sistemine eklenmiştir.",
                Hedef = DuyuruHedef.Herkes, Onemli = true, YayinlayanId = adminKullanici.Id,
                YayinTarihi = DateTime.UtcNow.AddDays(-3),
                IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            },
            new() {
                Baslik = "Öğretim Üyesi Görüşme Saatleri",
                Icerik = "Öğretim üyelerinin haftalık görüşme saatleri akademik takvimde güncellenmiştir.",
                Hedef = DuyuruHedef.Ogretmenler, Onemli = false, YayinlayanId = adminKullanici.Id,
                YayinTarihi = DateTime.UtcNow.AddDays(-5),
                IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            },
        };

        await context.Duyurular.AddRangeAsync(duyurular);
        await context.SaveChangesAsync();
    }

    // ── Yardımcı: 100'lük → HarfNotu ─────────────────────────────────────────
    private static HarfNotu HesaplaHarfNotu(decimal not) => not switch
    {
        >= 85 => HarfNotu.AA,
        >= 75 => HarfNotu.BA,
        >= 65 => HarfNotu.BB,
        >= 60 => HarfNotu.CB,
        >= 55 => HarfNotu.CC,
        >= 50 => HarfNotu.DC,
        >= 45 => HarfNotu.DD,
        _ => HarfNotu.FF
    };

    private static decimal HesaplaKatsayi(HarfNotu harf) => harf switch
    {
        HarfNotu.AA => 4.0m,
        HarfNotu.BA => 3.5m,
        HarfNotu.BB => 3.0m,
        HarfNotu.CB => 2.5m,
        HarfNotu.CC => 2.0m,
        HarfNotu.DC => 1.5m,
        HarfNotu.DD => 1.0m,
        _ => 0.0m
    };

    private static string Temizle(string s) => s
        .Replace("ı", "i").Replace("İ", "i").Replace("ğ", "g").Replace("Ğ", "g")
        .Replace("ü", "u").Replace("Ü", "u").Replace("ş", "s").Replace("Ş", "s")
        .Replace("ö", "o").Replace("Ö", "o").Replace("ç", "c").Replace("Ç", "c")
        .Replace(" ", "").ToLower();
}