using StudentManagement.Core.Enums;

namespace StudentManagement.Services.ViewModels.OgrenciIsleri;

// ── Dashboard ─────────────────────────────────────────────────────────────────
public class OgrenciIsleriDashboardViewModel
{
    public string DonemAdi              { get; set; } = string.Empty;
    public string AktifDonemAdi         { get; set; } = string.Empty;
    public int    BekleyenKayitTalep    { get; set; }
    public int    BekleyenTalepSayisi   { get; set; }
    public int    BekleyenBelgeTalebi   { get; set; }
    public int    BekleyenBelgeSayisi   { get; set; }
    public int    BugunkuOnay           { get; set; }
    public int    BugunkuOnaylananSayisi { get; set; }
    public int    BugunkuRed            { get; set; }
    public int    ToplamAktifOgrenci    { get; set; }
    public List<KayitTalepOzetViewModel>  SonKayitTalepleri  { get; set; } = new();
    public List<TalepOzetViewModel>       SonTalepler        { get; set; } = new();
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

// ── Kayıt Talep ──────────────────────────────────────────────────────────────
public class KayitTalepOzetViewModel
{
    public int      OgrenciDersId  { get; set; }
    public string   OgrenciNo      { get; set; } = string.Empty;
    public string   OgrenciAdi     { get; set; } = string.Empty;
    public string   BolumAdi       { get; set; } = string.Empty;
    public string   BolumKodu      { get; set; } = string.Empty;
    public string   DersKodu       { get; set; } = string.Empty;
    public string   DersAdi        { get; set; } = string.Empty;
    public string   DonemAdi       { get; set; } = string.Empty;
    public DateTime TalepTarihi    { get; set; }
    public string   DurumAdi       { get; set; } = string.Empty;
}

public class KayitTalepDetayViewModel : KayitTalepOzetViewModel
{
    public string   OgretmenAdi    { get; set; } = string.Empty;
    public int      Akts           { get; set; }
    public int      KayitliSayisi  { get; set; }
    public int      MaxKontenjan   { get; set; }
    public decimal? Gano           { get; set; }
    public int      MevcutAktsYuku { get; set; }
    public string   Gun            { get; set; } = string.Empty;
    public TimeSpan? BaslangicSaati { get; set; }
    public TimeSpan? BitisSaati     { get; set; }
    public string   Derslik        { get; set; } = string.Empty;
}

public class KayitOnayRedViewModel
{
    public int    OgrenciDersId { get; set; }
    public bool   Onayla        { get; set; }
    public string? RedNedeni    { get; set; }
}

// ── Toplu onay/red ────────────────────────────────────────────────────────────
public class TopluKayitOnayViewModel
{
    public int?   DonemId         { get; set; }
    public int?   BolumId         { get; set; }
    public string DonemAdi        { get; set; } = string.Empty;
    public List<KayitTalepDetayViewModel> Talepler { get; set; } = new();

    // Filtreler için dropdown
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> DonemListesi { get; set; } = new();
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> BolumListesi { get; set; } = new();
}

// ── Belge Talebi ─────────────────────────────────────────────────────────────
public class BelgeTalepOzetViewModel
{
    public int        Id             { get; set; }
    public string     OgrenciNo      { get; set; } = string.Empty;
    public string     OgrenciAdi     { get; set; } = string.Empty;
    public string     BolumAdi       { get; set; } = string.Empty;
    public BelgeTur   BelgeTur       { get; set; }
    public string     BelgeTurAdi    { get; set; } = string.Empty;
    public int        Adet           { get; set; }
    public BelgeDurum Durum          { get; set; }
    public string     BelgeDurumAdi  { get; set; } = string.Empty;
    public DateTime   CreatedAt      { get; set; }
    public string?    Aciklama       { get; set; }
    public string     IslemYapanAdi  { get; set; } = string.Empty;
}

public class BelgeTalepGuncelleViewModel
{
    public int        Id             { get; set; }
    public int        BelgeTalebiId  { get; set; }
    public string     YeniBelgeDurum { get; set; } = string.Empty;
    public BelgeDurum Durum          { get; set; }
    public string?    SonucNotu      { get; set; }
    public DateTime?  TeslimTarihi    { get; set; }
}

// ── Öğrenci Arama / Profil ────────────────────────────────────────────────────
public class OgrenciAramaViewModel
{
    public string? Arama          { get; set; }
    public int?    BolumId        { get; set; }
    public OgrenciDurum? Durum    { get; set; }
    public List<OgrenciIsleriOgrenciViewModel> Sonuclar { get; set; } = new();
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> BolumListesi { get; set; } = new();
}

public class OgrenciIsleriOgrenciViewModel
{
    public int          OgrenciId    { get; set; }
    public string       OgrenciNo    { get; set; } = string.Empty;
    public string       AdSoyad      { get; set; } = string.Empty;
    public string       BolumAdi     { get; set; } = string.Empty;
    public int          SinifSeviyesi { get; set; }
    public decimal?     Gano         { get; set; }
    public int          TamamlananAkts { get; set; }
    public OgrenciDurum Durum        { get; set; }
    public string       Email        { get; set; } = string.Empty;
}

// ── Duyuru (Öğrenci İşleri yayınlayabilir) ───────────────────────────────────
public class DuyuruCreateViewModel
{
    public string      Baslik           { get; set; } = string.Empty;
    public string      Icerik           { get; set; } = string.Empty;
    public DuyuruHedef Hedef            { get; set; } = DuyuruHedef.TumOgrenciler;
    public int?        HedefBolumId     { get; set; }
    public bool        Onemli           { get; set; }
    public DateTime?   BitisTarihi      { get; set; }
    public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> BolumListesi { get; set; } = new();
}
public class TalepViewModel : KayitTalepOzetViewModel { }
public class TalepDetayViewModel : KayitTalepDetayViewModel { }
public class BelgeTalebiViewModel : BelgeTalepOzetViewModel { }
public class BelgeDurumGuncelleViewModel : BelgeTalepGuncelleViewModel { }
public class OgrenciDetayViewModel : OgrenciIsleriOgrenciViewModel { }

public class OgrenciAraViewModel : OgrenciAramaViewModel { }

public class TumTaleplerViewModel
{
    public IEnumerable<TalepDetayViewModel> DersKayitTalepleri { get; set; } = new List<TalepDetayViewModel>();
    public IEnumerable<BelgeTalebiViewModel> BelgeTalepleri { get; set; } = new List<BelgeTalebiViewModel>();
}
