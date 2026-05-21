namespace StudentManagement.Core.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    // Hata veren 2 parametreli constructor'ı ekliyoruz
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) bulunamadı.") { }
}