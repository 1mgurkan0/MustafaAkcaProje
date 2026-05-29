using StudentManagement.Core.Enums;

namespace StudentManagement.Services.ViewModels.Admin;

// ═══════════════════════════════════════════════════════════════
// SHARED / SELECT
// ═══════════════════════════════════════════════════════════════
public class KullaniciOzetViewModel
{
    public int    Id     { get; set; }
    public string TamAd  { get; set; } = string.Empty;
    public string RolAdi { get; set; } = string.Empty;

    // Select list için
    public string DisplayText => TamAd;
}

// ═══════════════════════════════════════════════════════════════
// DASHBOARD
// ═══════════════════════════════════════════════════════════════
public class AdminDashboardViewModel
{
    public int    ToplamOgrenci        { get; set; }
    public int    ToplamOgretmen       { get; set; }
    public int    ToplamBolum          { get; set; }
    public int    ToplamDers           { get; set; }
    public int    ToplamDersAtama      { get; set; }
    public int    BekleyenTalepSayisi  { get; set; }
    public string AktifDonemAdi        { get; set; } = string.Empty;

    public List<TalepOzetViewModel>  SonTalepler           { get; set; } = new();
    public List<OgrenciViewModel>    SonOgrenciler         { get; set; } = new();
    public List<BolumViewModel>      BolumIstatistikleri   { get; set; } = new();
}

public class TalepOzetViewModel
{
    public int      OgrenciDersId { get; set; }
    public string   OgrenciAdi    { get; set; } = string.Empty;
    public string   OgrenciNo     { get; set; } = string.Empty;
    public string   DersAdi       { get; set; } = string.Empty;
    public string   DersKodu      { get; set; } = string.Empty;
    public DateTime TalepTarihi   { get; set; }
}

// ═══════════════════════════════════════════════════════════════
// BÖLÜM
// ═══════════════════════════════════════════════════════════════
public class BolumViewModel
{
    public int    Id               { get; set; }
    public string BolumKodu        { get; set; } = string.Empty;
    public string BolumAdi         { get; set; } = string.Empty;
    public int    MinMezuniyetAkts { get; set; }
    public int    OgrenciSayisi    { get; set; }
    public int    DersSayisi       { get; set; }
    public bool   IsActive         { get; set; }
}

public class BolumOlusturViewModel
{
    public string BolumKodu        { get; set; } = string.Empty;
    public string BolumAdi         { get; set; } = string.Empty;
    public int    MinMezuniyetAkts { get; set; } = 240;
    public string? Aciklama        { get; set; }
}

public class BolumDuzenleViewModel
{
    public int    Id               { get; set; }
    public string BolumKodu        { get; set; } = string.Empty;
    public string BolumAdi         { get; set; } = string.Empty;
    public int    MinMezuniyetAkts { get; set; }
    public string? Aciklama        { get; set; }
    public bool   IsActive         { get; set; }
}

public class BolumSelectViewModel
{
    public int    Id          { get; set; }
    public string DisplayText { get; set; } = string.Empty;
}

// ═══════════════════════════════════════════════════════════════
// DÖNEM
// ═══════════════════════════════════════════════════════════════
public class DonemViewModel
{
    public int      Id                 { get; set; }
    public string   DonemKodu          { get; set; } = string.Empty;
    public int      Yil                { get; set; }
    public string   DonemTurAdi        { get; set; } = string.Empty;
    public bool     AktifMi            { get; set; }
    public bool     IsActive           { get; set; }
    public DateTime DersKayitBaslangic { get; set; }
    public DateTime DersKayitBitis     { get; set; }
    public int      DersAtamaSayisi    { get; set; }
}

public class DonemOlusturViewModel
{
    public string   DonemKodu          { get; set; } = string.Empty;
    public int      Yil                { get; set; } = DateTime.Now.Year;
    public DonemTur DonemTur           { get; set; }
    public DateTime BaslangicTarihi    { get; set; } = DateTime.Today;
    public DateTime BitisTarihi        { get; set; } = DateTime.Today.AddMonths(4);
    public DateTime DersKayitBaslangic { get; set; }
    public DateTime DersKayitBitis     { get; set; }
}

public class DonemDuzenleViewModel
{
    public int      Id                 { get; set; }
    public string   DonemKodu          { get; set; } = string.Empty;
    public int      Yil                { get; set; }
    public DonemTur DonemTur           { get; set; }
    public bool     AktifMi            { get; set; }
    public DateTime BaslangicTarihi    { get; set; }
    public DateTime BitisTarihi        { get; set; }
    public DateTime DersKayitBaslangic { get; set; }
    public DateTime DersKayitBitis     { get; set; }
}

public class DonemSelectViewModel
{
    public int    Id          { get; set; }
    public string DisplayText { get; set; } = string.Empty;
}

// ═══════════════════════════════════════════════════════════════
// DERS
// ═══════════════════════════════════════════════════════════════
public class DersViewModel
{
    public int    Id           { get; set; }
    public string DersKodu     { get; set; } = string.Empty;
    public string DersAdi      { get; set; } = string.Empty;
    public int    Akts         { get; set; }
    public int    MaxKontenjan { get; set; }
    public string BolumAdi     { get; set; } = string.Empty;
    public string BolumKodu    { get; set; } = string.Empty;
    public bool   IsActive     { get; set; }
}

public class DersOlusturViewModel
{
    public List<StudentManagement.Services.ViewModels.Admin.BolumSelectViewModel> BolumListesi { get; set; } = new();
    public string  DersKodu     { get; set; } = string.Empty;
    public string  DersAdi      { get; set; } = string.Empty;
    public int     Akts         { get; set; }
    public int     MaxKontenjan { get; set; }
    public int     BolumId      { get; set; }
    public string? Aciklama     { get; set; }
}

public class DersDuzenleViewModel
{
    public int     Id           { get; set; }
    public string  DersKodu     { get; set; } = string.Empty;
    public string  DersAdi      { get; set; } = string.Empty;
    public int     Akts         { get; set; }
    public int     MaxKontenjan { get; set; }
    public int     BolumId      { get; set; }
    public string? Aciklama     { get; set; }
    public bool    IsActive     { get; set; }
}

public class DersSelectViewModel
{
    public int    Id          { get; set; }
    public string DisplayText { get; set; } = string.Empty;
}

// ═══════════════════════════════════════════════════════════════
// DERS ATAMA
// ═══════════════════════════════════════════════════════════════
public class DersAtamaViewModel
{
    public int      Id                   { get; set; }
    public string   DersAdi              { get; set; } = string.Empty;
    public string   DersKodu             { get; set; } = string.Empty;
    public int      Akts                 { get; set; }
    public int      MaxKontenjan         { get; set; }
    public string   OgretmenAdi          { get; set; } = string.Empty;
    public string   OgretmenUnvanliAd    { get; set; } = string.Empty;
    public string   DonemAdi             { get; set; } = string.Empty;
    public string   BolumAdi             { get; set; } = string.Empty;
    public string   GunAdi               { get; set; } = string.Empty;
    public TimeSpan? BaslangicSaati      { get; set; }
    public TimeSpan? BitisSaati          { get; set; }
    public string   Derslik              { get; set; } = string.Empty;
    public int      KayitliOgrenciSayisi { get; set; }
    public double   DolulukOrani         { get; set; }
}

public class DersAtamaOlusturViewModel
{
    public int       DersId         { get; set; }
    public int       DonemId        { get; set; }
    public int       OgretmenId     { get; set; }
    public DersGun   Gun            { get; set; }
    public TimeSpan? BaslangicSaati { get; set; }
    public TimeSpan? BitisSaati     { get; set; }
    public string    Derslik        { get; set; } = string.Empty;
}

public class DersAtamaDetayViewModel
{
    public int      DersAtamaId          { get; set; }
    public string   DersAdi              { get; set; } = string.Empty;
    public string   DersKodu             { get; set; } = string.Empty;
    public int      Akts                 { get; set; }
    public int      MaxKontenjan         { get; set; }
    public string   OgretmenUnvanliAd    { get; set; } = string.Empty;
    public string   DonemAdi             { get; set; } = string.Empty;
    public string   GunAdi               { get; set; } = string.Empty;
    public TimeSpan? BaslangicSaati      { get; set; }
    public TimeSpan? BitisSaati          { get; set; }
    public string   Derslik              { get; set; } = string.Empty;
    public int      KayitliOgrenciSayisi { get; set; }
    public List<DersAtamaOgrenciViewModel> KayitliOgrenciler { get; set; } = new();
}

public class DersAtamaOgrenciViewModel
{
    public int      OgrenciId    { get; set; }
    public string   OgrenciNo    { get; set; } = string.Empty;
    public string   OgrenciAdi   { get; set; } = string.Empty;
    public string   BolumKodu    { get; set; } = string.Empty;
    public decimal? VizeNotu     { get; set; }
    public decimal? FinalNotu    { get; set; }
    public decimal? GenelNot     { get; set; }
    public string   HarfNotuAdi  { get; set; } = "—";
    public string   DurumAdi     { get; set; } = string.Empty;
}

// ═══════════════════════════════════════════════════════════════
// ÖĞRENCİ
// ═══════════════════════════════════════════════════════════════
public class OgrenciViewModel
{
    public int      OgrenciId { get; set; }
    public string   OgrenciNo { get; set; } = string.Empty;
    public string   TamAd     { get; set; } = string.Empty;
    public string   Email     { get; set; } = string.Empty;
    public string   BolumAdi  { get; set; } = string.Empty;
    public string   BolumKodu { get; set; } = string.Empty;
    public decimal? Gano      { get; set; }
    public string   DurumAdi  { get; set; } = string.Empty;
    public string OgrenciDurumAdi { get; set; } = string.Empty;
}

public class OgrenciDetayViewModel : OgrenciViewModel
{
    public int    TamamlananAkts  { get; set; }
    public int    AktifDersSayisi { get; set; }
}
// ═══════════════════════════════════════════════════════════════
// AUDIT LOG
// ═══════════════════════════════════════════════════════════════
public class AuditLogViewModel
{
    public int      Id           { get; set; }
    public string   KullaniciAdi { get; set; } = string.Empty;
    public string   ActionAdi    { get; set; } = string.Empty;
    public string   Entity       { get; set; } = string.Empty;
    public int?     EntityId     { get; set; }
    public string?  EskiDeger    { get; set; }
    public string?  YeniDeger    { get; set; }
    public string?  Aciklama     { get; set; }
    public DateTime Timestamp    { get; set; }
}

public class AuditLogListViewModel
{
    public List<AuditLogViewModel> Logs        { get; set; } = new();
    public int                     ToplamKayit { get; set; }
    public int                     Sayfa       { get; set; }
    public int                     SayfaBoyutu { get; set; }
    public int ToplamSayfa => (int)Math.Ceiling((double)ToplamKayit / SayfaBoyutu);
}
public class DersAtamaDetailViewModel : DersAtamaDetayViewModel { }
