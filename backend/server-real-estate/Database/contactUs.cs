using Microsoft.AspNetCore.Identity;

namespace server_real_estate.Database;

public class ContactUs
{
    public ContactUs()
    {

        Id = Guid.NewGuid();
        Name = string.Empty;
        Address = string.Empty;
        Email = string.Empty;
        PhoneNumber = string.Empty;
        Message = string.Empty;
    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Message { get; set; }

}