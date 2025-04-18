using Microsoft.AspNetCore.Identity;

namespace server_real_estate.Database;

public class ContactUs
{
    public ContactUs()
    {

        Name = string.Empty;
        Address = string.Empty;
        Email = string.Empty;
        Phone = string.Empty;
        Message = string.Empty;


    }


    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Message { get; set; }

}