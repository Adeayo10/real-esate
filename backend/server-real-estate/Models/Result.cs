namespace server_real_estate.Model;
public class Result<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public static Result<T> Ok(T data, string message) =>
        new()
        { Success = true, Data = data, Message = message };

    public static Result<T> Fail(string message) =>
        new()
        { Success = false, Message = message };
}