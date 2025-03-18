using System.Collections.Generic;

namespace server_real_estate.Model;
public class UserResponse<T>
{
    public bool Success { get; }
    public string Message { get; }
    public T Data { get; }
    public int StatusCode { get; }
    public List<string> Errors { get; }

    public UserResponse(T data, bool success, string message, int statusCode, List<string> errors)
    {
        Success = success;
        Message = message;
        Data = data;
        StatusCode = statusCode;
        Errors = errors ?? new List<string>();
    }

    private static UserResponse<T> CreateResponse(T data, bool success, string message, int statusCode, List<string> errors = null) 
        => new(data, success, message, statusCode, errors);

    public static UserResponse<T> Ok(T data, string message = "Operation successful")
        => CreateResponse(data, true, message, 200);
    
    public static UserResponse<T> Created(T data, string message = "Resource created successfully") 
        => CreateResponse(data, true, message, 201);
    
    public static UserResponse<T> BadRequest(string message = "Bad request", List<string> errors = null) 
        => CreateResponse(default, false, message, 400, errors);
    
    public static UserResponse<T> NotFound(string message = "Resource not found") 
        => CreateResponse(default, false, message, 404);
    
    public static UserResponse<T> ServerError(string message = "Internal server error") 
        => CreateResponse(default, false, message, 500);
}
