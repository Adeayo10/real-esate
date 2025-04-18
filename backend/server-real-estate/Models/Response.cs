namespace server_real_estate.Model;
public class SuccessResponse
{
    public string UserId { get; init; }
    public string Message { get; init; }
    public bool IsSuccessful { get; init; }
    public string StatusCode { get; init; }
    public string Token { get; init; } // Optional token
    public DateTime? Expires { get; init; } // Optional expiration date
    public string TokenType { get; init; } // Optional token type (e.g., Bearer)
    public string RefreshToken { get; init; } // Optional refresh token
    
}

public class ErrorResponse 
{
    public string Message { get; init; }
    public object Errors { get; init; }
    public bool IsSuccessful { get; init; }
    public string StatusCode { get; init; }
}
