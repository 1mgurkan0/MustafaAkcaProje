using StudentManagement.Core.Enums;

namespace StudentManagement.Services.ViewModels.Common;

public class DuyuruOlusturViewModel
{
    public int         Id              { get; set; }
    public string      Baslik          { get; set; } = string.Empty;
    public string      Icerik          { get; set; } = string.Empty;
    public DateTime?   YayinTarihi     { get; set; }
    public DateTime?   BitisTarihi     { get; set; }
    public DateTime    CreatedAt       { get; set; } // Added this as it's used in OgretmenPanelService
    public bool        Onemli          { get; set; }
    public string      YayinlayanAdi   { get; set; } = string.Empty;
    public string      HedefAdi        { get; set; } = string.Empty;
    public string?     DersAdi         { get; set; }
}


public class TalepRedViewModel
{
    public int    OgrenciDersId { get; set; }
    public string RedNedeni     { get; set; } = string.Empty;
}
