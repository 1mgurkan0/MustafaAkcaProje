namespace StudentManagement.Core.Enums;

/// <summary>
/// 100'lük nottan otomatik hesaplanan 4'lük harf notu.
/// AA=4.0, BA=3.5, BB=3.0, CB=2.5, CC=2.0, DC=1.5, DD=1.0, FF=0.0
/// </summary>
public enum HarfNotu
{
    FF  = 0,   // 0  - 44   → Başarısız
    DD  = 1,   // 45 - 49   → Koşullu Geçer
    DC  = 2,   // 50 - 54   → Koşullu Geçer
    CC  = 3,   // 55 - 59   → Geçer
    CB  = 4,   // 60 - 64   → Orta
    BB  = 5,   // 65 - 74   → İyi
    BA  = 6,   // 75 - 84   → Pekiyi
    AA  = 7,   // 85 - 100  → Mükemmel
    NA  = 8,   // Devamsızlıktan Kalma (Not girilmedi)
    MU  = 9,   // Muaf (önceki eğitimden sayıldı)
    EK  = 10   // Eksik (tamamlanmamış ödev/proje)
}
