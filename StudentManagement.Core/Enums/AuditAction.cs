namespace StudentManagement.Core.Enums;

public enum AuditAction
{
    // Mevcut
    Create           = 1,
    Update           = 2,
    Delete           = 3,
    Login            = 4,
    Logout           = 5,
    Register         = 6,
    DersAta          = 7,
    DersCikar        = 8,
    NotGuncelle      = 9,
    SifreDegistir    = 10,
    HesapKilitle     = 11,
    HesapAc          = 12,

    // UBYS - Ders Kayıt
    DersKayitTalep   = 13,   // Öğrenci ders kaydı talep etti
    DersKayitOnayla  = 14,   // Öğrenci işleri onayladı
    DersKayitReddet  = 15,   // Öğrenci işleri reddetti
    DersBirak        = 16,   // Öğrenci dersi bıraktı

    // UBYS - Not
    VizeNotuGir      = 17,
    FinalNotuGir     = 18,
    ButunlemeNotuGir = 19,

    // UBYS - Yoklama
    YoklamaAl        = 20,

    // UBYS - Yönetim
    DonemOlustur     = 21,
    DonemAktifYap    = 22,
    BolumOlustur     = 23,
    DersAtamaYap     = 24,

    // UBYS - Belge
    BelgeTalepOlustur = 25,
    BelgeTeslim       = 26,

    // UBYS - Duyuru
    DuyuruYayinla     = 27,

    // UBYS - Not (Generic)
    NotGir            = 28
}
