namespace server_real_estate.Model;
public class SuccessResponse
{
    public string UserId { get; init; }
    public string Message { get; init; }
    public bool IsSuccessful { get; init; }
    public string StatusCode { get; init; }
    public string Token { get; init; } // Optional token
}

public class ErrorResponse 
{
    public string Message { get; init; }
    public object Errors { get; init; }
    public bool IsSuccessful { get; init; }
    public string StatusCode { get; init; }
}
