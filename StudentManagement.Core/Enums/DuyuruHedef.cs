namespace StudentManagement.Core.Enums;

public enum DuyuruHedef
{
    Herkes        = 1,   // Tüm kullanıcılar
    Ogrenciler    = 2,   // Sadece öğrenciler
    Ogretmenler   = 3,   // Sadece öğretmenler
    OgrenciIsleri = 4,   // Öğrenci işleri personeli
    BolumOzeli    = 5,   // Belirli bir bölüm
    DersOzeli     = 6,   // Belirli bir ders (DersAtama bazlı)
    TumOgrenciler = 7,   // Ogrenciler ile aynı ama bazı yerlerde bu isimle kullanılmış
    Bolum         = 8    // BolumOzeli ile aynı ama bazı yerlerde bu isimle kullanılmış
}
