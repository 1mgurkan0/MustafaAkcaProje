# Üniversite Bilgi Yönetim Sistemi (UBYS)

Modern, hızlı ve responsive bir Üniversite Öğrenci Bilgi Yönetim Sistemi. Gelişmiş yetkilendirme altyapısı sayesinde Öğrenci, Öğretmen, Öğrenci İşleri ve Sistem Yöneticisi (Admin) olmak üzere 4 farklı rol ile uçtan uca akademik yönetim sağlar.

## 🚀 Özellikler

- **Çoklu Rol Desteği:** Admin, Öğretmen, Öğrenci, Öğrenci İşleri. Her rol için özel arayüz ve yetkilendirme.
- **Modern ve Responsive Tasarım:** Masaüstü, tablet ve mobil cihazlarla tam uyumlu, akıcı UI/UX deneyimi.
- **Ders ve Not Yönetimi:** Öğretmenler tarafından not girişi (Vize, Final) ve otomatik GANO/Harf notu hesaplaması.
- **Ders Atamaları ve Kayıtlar:** Öğrencilerin kendi bölümlerine ve aktif dönemlerine uygun dersleri seçebilmeleri.
- **Duyuru Sistemi:** Hedef kitle (Herkes, Sadece Öğrenciler, Sadece Öğretmenler) bazlı duyuru yayınlama yeteneği.
- **Kapsamlı Dashboardlar:** Rol bazlı istatistikler ve hızlı erişim menüleri.

## 🛠️ Kullanılan Teknolojiler

- **Backend:** C# / .NET 8, ASP.NET Core MVC
- **Veritabanı:** SQL Server, Entity Framework Core (Code-First)
- **Güvenlik:** BCrypt Password Hashing, Session bazlı özel yetkilendirme altyapısı
- **Frontend:** HTML5, CSS3 (Modern Viewport Ölçeklemeli Custom Tasarım), Vanilla JavaScript
- **Araçlar:** Serilog (Gelişmiş loglama), AutoMapper

## ⚙️ Kurulum ve Çalıştırma

Proje ortamını kendi bilgisayarınıza kurmak için aşağıdaki adımları izleyin:

### Gereksinimler
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB veya standart sürüm)

### 1. Veritabanı Ayarları
`StudentManagement.Web/appsettings.json` dosyasını açarak `DefaultConnection` kısmını kendi SQL Server bağlantı dizenize (Connection String) göre güncelleyin.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True;"
}
```

### 2. Projeyi Derleme ve Çalıştırma
Terminali açıp ana dizinde aşağıdaki komutu çalıştırın:
```bash
dotnet run --project StudentManagement.Web
```

*Not: Uygulama ilk kez ayağa kalktığında Code-First yapısı gereği veritabanınızı otomatik olarak oluşturacak (Migrate) ve varsayılan test/örnek verilerini (Seed) içeri aktaracaktır.*

## 🔐 İlk Kurulum ve Güvenlik

Sistem ilk ayağa kalktığında `DataSeeder` vasıtasıyla boş veritabanını gerekli temel verilerle (Bölümler, Dönemler) doldurur. İlk sistem yöneticisi (Admin) hesabının giriş bilgileri sistem kurulumunu yapan yetkiliye özeldir ve güvenlik politikaları gereği burada paylaşılmamaktadır. İlk kurulum sonrası admin hesabı bilgilerini veritabanı `Kullanicilar` tablosundan kontrol edebilir ve şifreyi derhal değiştirmelisiniz.

## 📱 Ekran Görüntüleri ve Mobil Uyumluluk

Sistem özel `viewport-fit=cover` ölçeklemesi kullanmaktadır. Ekran boyutu küçüldükçe sayfa font ve margin yapıları kendini otomatik olarak daraltır. Cep telefonlarında dahi masaüstü genişliğinde ve rahatlığında bir kullanım sunar. Sağdan sola açılan "Drawer" tip menüye sahiptir.

---
**Geliştirici Notu:** Bu proje; katmanlı mimari (Core, Data, Services, Web) prensiplerine sadık kalınarak, sürdürülebilir ve genişletilebilir bir altyapıyla inşa edilmiştir.
