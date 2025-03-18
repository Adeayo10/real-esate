namespace server_real_estate.Model;
public class Login
{

    public required string Email { get; set; }
    public required string Password { get; set; }

    public bool? RememberMe { get; set; }
}
