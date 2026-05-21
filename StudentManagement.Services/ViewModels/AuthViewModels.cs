namespace StudentManagement.Services.ViewModels.Auth;

public class LoginViewModel
{
    public string KullaniciAdi { get; set; } = string.Empty;
    public string Sifre        { get; set; } = string.Empty;
}

public class RegisterViewModel
{
    public string Ad           { get; set; } = string.Empty;
    public string Soyad        { get; set; } = string.Empty;
    public string KullaniciAdi { get; set; } = string.Empty;
    public string Email        { get; set; } = string.Empty;
    public string Sifre        { get; set; } = string.Empty;
    public string SifreTekrar  { get; set; } = string.Empty;
    public int    BolumId      { get; set; }
}
public class LoginSessionData
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int? OgrenciId { get; set; }
    public int? BolumId { get; set; }
}