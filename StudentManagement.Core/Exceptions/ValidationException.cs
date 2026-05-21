namespace StudentManagement.Core.Exceptions;
public class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }
    public ValidationException(IEnumerable<string> errors)
        : base("Bir veya daha fazla doğrulama hatası oluştu.")
    {
        Errors = errors.ToList().AsReadOnly();
    }
}
