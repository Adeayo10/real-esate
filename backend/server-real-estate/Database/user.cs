using Microsoft.AspNetCore.Identity;

namespace server_real_estate.Database;

public class User : IdentityUser
{
    public User()
    {
        // Id = Guid.NewGuid();
        FirstName = string.Empty;
        LastName = string.Empty;
        Password = string.Empty;
        Role = string.Empty;
        

    }

    // public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    
    public int TokenVersion {get; set;}
}