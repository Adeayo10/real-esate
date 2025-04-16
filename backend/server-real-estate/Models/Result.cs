namespace server_real_estate.Model;
public class Result<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public DateTime? Expires { get; set; }
    public string RefreshToken { get; set; }

    public static Result<T> Ok(T data, string message, DateTime? expires = null) =>
        new()
        { Success = true, Data = data, Message = message, Expires = expires };

    public static Result<T> Fail(string message) =>
        new()
        { Success = false, Message = message };
}

