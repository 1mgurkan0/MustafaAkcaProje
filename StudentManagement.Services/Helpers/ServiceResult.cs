namespace StudentManagement.Services.Helpers;

public class ServiceResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public static ServiceResult Ok(string message = "") => new() { IsSuccess = true, Message = message };
    public static ServiceResult Fail(string message) => new() { IsSuccess = false, Message = message };
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }

    public static ServiceResult<T> Ok(T data, string message = "") => new() { IsSuccess = true, Data = data, Message = message };
    public new static ServiceResult<T> Fail(string message) => new() { IsSuccess = false, Message = message };
}