using StudentManagement.Core.Enums;

namespace StudentManagement.Services.ViewModels.Ogrenci;

// ─── DASHBOARD ────────────────────────────────────────────────────────────────
public class OgrenciDashboardViewModel
{
    public string   TamAd               { get; set; } = string.Empty;
    public string   OgrenciNo           { get; set; } = string.Empty;
    public string   BolumAdi            { get; set; } = string.Empty;
    public string   AktifDonemAdi       { get; set; } = string.Empty;
    public decimal? Gano                { get; set; }
    public int      TamamlananAkts      { get; set; }
    public int      MinMezuniyetAkts    { get; set; } = 240;
    public int      AktifDersSayisi     { get; set; }
    public int      AktifAktsYuku       { get; set; }
    public int      YaklasanSinavSayisi { get; set; }

    public List<OgrenciDersViewModel>                                               AktifDersler     { get; set; } = new();
    public List<StudentManagement.Services.ViewModels.Ogretmen.SinavViewModel> YaklasanSinavlar { get; set; } = new();
}

// ─── DERS KAYIT ───────────────────────────────────────────────────────────────
public class DersKayitViewModel
{
    public string   DonemAdi       { get; set; } = string.Empty;
    public bool     KayitAcik      { get; set; }
    public DateTime KayitBaslangic { get; set; }
    public DateTime KayitBitis     { get; set; }
    public int      MevcutAktsYuku { get; set; }

    public List<DersKayitSatirViewModel> MevcutDersler { get; set; } = new();
}

public class DersKayitSatirViewModel
{
    public int      DersAtamaId    { get; set; }
    public string   DersKodu       { get; set; } = string.Empty;
    public string   DersAdi        { get; set; } = string.Empty;
    public string   BolumAdi       { get; set; } = string.Empty;
    public string   OgretmenAdi    { get; set; } = string.Empty;
    public int      Akts           { get; set; }
    public string   Gun            { get; set; } = string.Empty;
    public TimeSpan? BaslangicSaati { get; set; }
    public TimeSpan? BitisSaati     { get; set; }
    public string   Derslik        { get; set; } = string.Empty;
    public int      MaxKontenjan   { get; set; }
    public int      KayitliSayisi  { get; set; }
    public bool     ZatenKayitli   { get; set; }
    public bool     TalepVar       { get; set; }
}

public class DersKayitTalepViewModel
{
    public int DersAtamaId { get; set; }
}

// ─── ÖĞRENCİ DERS ────────────────────────────────────────────────────────────
public class OgrenciDersViewModel
{
    public string Saat { get; set; } = string.Empty;
    public int      OgrenciDersId  { get; set; }
    public int      DersAtamaId    { get; set; }
    public string   DersAdi        { get; set; } = string.Empty;
    public string   DersKodu       { get; set; } = string.Empty;
    public int      Akts           { get; set; }
    public string   OgretmenAdi    { get; set; } = string.Empty;
    public string   Gun            { get; set; } = string.Empty;
    public TimeSpan? BaslangicSaati { get; set; }
    public TimeSpan? BitisSaati     { get; set; }
    public string   Derslik        { get; set; } = string.Empty;
    public decimal? VizeNotu       { get; set; }
    public decimal? FinalNotu      { get; set; }
    public decimal? GenelNot       { get; set; }
    public string   HarfNotuAdi    { get; set; } = "-";
    public string   DurumAdi       { get; set; } = string.Empty;
}

// ─── TRANSKRİPT ───────────────────────────────────────────────────────────────
public class TranskriptViewModel
{
    public string   TamAd                { get; set; } = string.Empty;
    public string   OgrenciNo            { get; set; } = string.Empty;
    public string   BolumAdi             { get; set; } = string.Empty;
    public decimal? Gano                 { get; set; }
    public int      TamamlananAkts       { get; set; }
    public int      TamamlananDersSayisi { get; set; }
    public int      MinMezuniyetAkts     { get; set; }

    public List<TranskriptDonemGrubuViewModel> DonemGruplari { get; set; } = new();
}

public class TranskriptDonemGrubuViewModel
{
    public string   DonemAdi    { get; set; } = string.Empty;
    public int      ToplamAkts  { get; set; }
    public decimal? DonemGanosu { get; set; }

    public List<TranskriptSatirViewModel> Dersler { get; set; } = new();
}

public class TranskriptSatirViewModel
{
    public string   DersKodu    { get; set; } = string.Empty;
    public string   DersAdi     { get; set; } = string.Empty;
    public int      Akts        { get; set; }
    public string   DonemAdi    { get; set; } = string.Empty;
    public decimal? VizeNotu    { get; set; }
    public decimal? FinalNotu   { get; set; }
    public decimal? GenelNot    { get; set; }
    public string   HarfNotuAdi { get; set; } = "-";
}

// ─── DERS PROGRAMI ────────────────────────────────────────────────────────────
public class DersProgramiViewModel
{
    public string DonemAdi { get; set; } = string.Empty;
    public List<DersProgramiSatirViewModel> Dersler { get; set; } = new();
}

public class DersProgramiSatirViewModel
{
    public int      DersAtamaId    { get; set; }
    public string   DersAdi        { get; set; } = string.Empty;
    public string   DersKodu       { get; set; } = string.Empty;
    public int      Akts           { get; set; }
    public string   OgretmenAdi    { get; set; } = string.Empty;
    public string   Gun            { get; set; } = string.Empty;
    public int      GunSiraNo      { get; set; }
    public TimeSpan? BaslangicSaati { get; set; }
    public TimeSpan? BitisSaati     { get; set; }
    public string   Derslik        { get; set; } = string.Empty;
}

// ─── SINAV TAKVİMİ ────────────────────────────────────────────────────────────
public class SinavTakvimiViewModel
{
    public string DonemAdi { get; set; } = string.Empty;
    public List<StudentManagement.Services.ViewModels.Ogretmen.SinavViewModel> Sinavlar { get; set; } = new();
}

// ─── BELGELER ─────────────────────────────────────────────────────────────────
public class BelgelerViewModel
{
    public List<BelgeTalebiViewModel> Talepler { get; set; } = new();
}

public class BelgeTalebiViewModel
{
    public int      Id            { get; set; }
    public string   OgrenciAdi    { get; set; } = string.Empty;
    public string   OgrenciNo     { get; set; } = string.Empty;
    public string   BelgeTurAdi   { get; set; } = string.Empty;
    public string   BelgeDurumAdi { get; set; } = string.Empty;
    public string?  Aciklama      { get; set; }
    public string   IslemYapanAdi { get; set; } = string.Empty;
    public DateTime CreatedAt     { get; set; }
}

public class BelgeTalebiOlusturViewModel
{
    public BelgeTur BelgeTur  { get; set; }
    public string?  Aciklama  { get; set; }
}
