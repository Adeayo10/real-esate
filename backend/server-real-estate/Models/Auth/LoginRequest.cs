namespace server_real_estate.Model;
public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    // public bool RememberMe { get; set; } // Determines if the cookie is persistent
    public bool UseBearerToken { get; set; } // Determines if Bearer tokens should be used
}