using StudentManagement.Core.Enums;

namespace StudentManagement.Services.ViewModels.Common;

// NOT: DuyuruOlusturViewModel Ogretmen namespace'inde tanımlanmıştır.
// Ambiguous reference hatasını önlemek için burada kullanılmaz.

public class TalepRedViewModel
{
    public int    OgrenciDersId { get; set; }
    public string RedNedeni     { get; set; } = string.Empty;
}
