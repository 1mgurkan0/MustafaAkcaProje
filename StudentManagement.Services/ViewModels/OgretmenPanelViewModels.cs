using StudentManagement.Core.Enums;

namespace StudentManagement.Services.ViewModels.Ogretmen;

// ─── DASHBOARD ────────────────────────────────────────────────────────────────
public class OgretmenDashboardViewModel
{
    public string  UnvanliAd            { get; set; } = string.Empty;
    public string  AktifDonemAdi        { get; set; } = string.Empty;
    public int     AktifDersSayisi      { get; set; }
    public int     ToplamOgrenciSayisi  { get; set; }
    public int     YaklasanSinavSayisi  { get; set; }
    public int     DuyuruSayisi         { get; set; }

    public List<DersAtamaOzetViewModel> AktifDersler     { get; set; } = new();
    public List<SinavViewModel>         YaklasanSinavlar { get; set; } = new();
}

// ─── DERS ATAMA ÖZET ──────────────────────────────────────────────────────────
public class DersAtamaOzetViewModel
{
    public int    DersAtamaId          { get; set; }
    public string DersAdi              { get; set; } = string.Empty;
    public string DersKodu             { get; set; } = string.Empty;
    public int    Akts                 { get; set; }
    public string Gun                  { get; set; } = string.Empty;
    public string Saat                 { get; set; } = string.Empty;
    public string Derslik              { get; set; } = string.Empty;
    public int    KayitliOgrenciSayisi { get; set; }
    public int    MaxKontenjan         { get; set; }
    public string DonemAdi             { get; set; } = string.Empty;
}

// ─── DERS DETAY ───────────────────────────────────────────────────────────────
public class DersDetayViewModel
{
    public int    DersAtamaId          { get; set; }
    public string DersAdi              { get; set; } = string.Empty;
    public string DersKodu             { get; set; } = string.Empty;
    public string DonemAdi             { get; set; } = string.Empty;
    public string Derslik              { get; set; } = string.Empty;
    public int    KayitliOgrenciSayisi { get; set; }
    public int    YoklamaSayisi        { get; set; }
    public int    SinavSayisi          { get; set; }
    public int    DuyuruSayisi         { get; set; }

    public List<OgrenciNotSatirViewModel> Ogrenciler { get; set; } = new();
}

// ─── NOT GİR ──────────────────────────────────────────────────────────────────
public class NotGirViewModel
{
    public int    DersAtamaId { get; set; }
    public string DersAdi     { get; set; } = string.Empty;
    public string DersKodu    { get; set; } = string.Empty;
    public string DonemAdi    { get; set; } = string.Empty;

    public List<OgrenciNotSatirViewModel> Notlar { get; set; } = new();
}

public class OgrenciNotSatirViewModel
{
    public int      OgrenciDersId { get; set; }
    public int      OgrenciId     { get; set; }
    public string   OgrenciNo     { get; set; } = string.Empty;
    public string   OgrenciAdi    { get; set; } = string.Empty;
    public decimal? VizeNotu      { get; set; }
    public decimal? FinalNotu     { get; set; }
    public decimal? ButunlemeNotu { get; set; }
    public decimal? GenelNot      { get; set; }
    public string   HarfNotuAdi   { get; set; } = "-";
    public double   DevamYuzdesi  { get; set; }
}

// ─── YOKLAMA ──────────────────────────────────────────────────────────────────
public class YoklamaGirViewModel
{
    public int      DersAtamaId   { get; set; }
    public string   DersAdi       { get; set; } = string.Empty;
    public string   DonemAdi      { get; set; } = string.Empty;
    public DateTime YoklamaTarihi { get; set; } = DateTime.Today;

    public List<OgrenciYoklamaSatirViewModel> OgrenciListesi { get; set; } = new();
}

public class OgrenciYoklamaSatirViewModel
{
    public int    OgrenciId  { get; set; }
    public string OgrenciNo  { get; set; } = string.Empty;
    public string OgrenciAdi { get; set; } = string.Empty;
    public bool   Geldi      { get; set; } = true;
}

public class YoklamaViewModel
{
    public int      Id            { get; set; }
    public int      DersAtamaId   { get; set; }
    public string   DersAdi       { get; set; } = string.Empty;
    public DateTime YoklamaTarihi { get; set; }

    public List<OgrenciYoklamaViewModel> OgrenciYoklamalar { get; set; } = new();
}

public class OgrenciYoklamaViewModel
{
    public int    OgrenciId  { get; set; }
    public string OgrenciNo  { get; set; } = string.Empty;
    public string OgrenciAdi { get; set; } = string.Empty;
    public bool   Geldi      { get; set; }
}

// ─── SINAV ────────────────────────────────────────────────────────────────────
public class SinavViewModel
{
    public int      Id            { get; set; }
    public string   DersAdi       { get; set; } = string.Empty;
    public string   DersKodu      { get; set; } = string.Empty;
    public string   SinavTurAdi   { get; set; } = string.Empty;
    public DateTime SinavTarihi   { get; set; }
    public string   Derslik       { get; set; } = string.Empty;
    public string   Aciklama      { get; set; } = string.Empty;
}

public class SinavlarViewModel
{
    public List<SinavViewModel> Sinavlar { get; set; } = new();
}

public class SinavEkleViewModel
{
    public int      DersAtamaId { get; set; }
    public SinavTur SinavTur    { get; set; }
    public DateTime SinavTarihi { get; set; }
    public string?  Derslik     { get; set; }
    public string?  Aciklama    { get; set; }
}

public class TopluNotGirisiViewModel { }
public class YoklamaAlViewModel : YoklamaGirViewModel { }
public class SinavCreateViewModel : SinavEkleViewModel { }
public class DuyuruOlusturViewModel {
    public StudentManagement.Core.Enums.DuyuruHedef Hedef { get; set; }
    public int? HedefDersAtamaId { get; set; }
    public string Baslik { get; set; } = string.Empty; 
    public string Icerik { get; set; } = string.Empty;
    public int Id { get; set; }
    public string HedefAdi { get; set; } = string.Empty;
    public bool Onemli { get; set; }
    public string DersAdi { get; set; } = string.Empty;
    public string YayinlayanAdi { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}


