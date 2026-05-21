using StudentManagement.Core.Constants;

namespace StudentManagement.Services.Helpers;

public static class PasswordHelper
{
    public static string Hash(string plainPassword)
        => BCrypt.Net.BCrypt.HashPassword(plainPassword, AppConstants.BcryptWorkFactor);

    public static bool Verify(string plainPassword, string hashedPassword)
        => BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);

    public static bool MeetsPolicy(string password)
    {
        if (password.Length < AppConstants.PasswordMinLength) return false;
        if (!password.Any(char.IsUpper))  return false;
        if (!password.Any(char.IsLower))  return false;
        if (!password.Any(char.IsDigit))  return false;
        if (!password.Any(ch => !char.IsLetterOrDigit(ch))) return false;
        return true;
    }
}
