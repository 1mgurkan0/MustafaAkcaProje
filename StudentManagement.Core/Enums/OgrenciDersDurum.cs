namespace StudentManagement.Core.Enums;

public enum OgrenciDersDurum
{
    // Mevcut
    Devam        = 1,
    Tamamlandi   = 2,
    Cekildi      = 3,
    Donduruldu   = 4,

    // UBYS - Kayıt süreci
    Talep        = 5,   // Öğrenci talep oluşturdu, onay bekliyor
    Reddedildi   = 6    // Öğrenci işleri reddetti
}
