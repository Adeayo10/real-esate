namespace server_real_estate.Model;
public class RegisterRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    // Add other properties as needed
}