using server_real_estate.Model;
using server_real_estate.Database;

namespace server_real_estate.Services;

public interface IContactService
{
    Task<Result<string>> CreateContactUsAsync(ContactUsRequest contactUsRequest);
}

public class ContactService(IRealEstatateDbContext context) : IContactService
{
    private readonly IRealEstatateDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    public async Task<Result<string>> CreateContactUsAsync(ContactUsRequest contactUsRequest)
    {
        if (contactUsRequest == null)
        {
            return Result<string>.Fail("Contact request is null");
        }

        //map the request to the entity
        var contactUs = new ContactUs
        {
            Id = Guid.NewGuid(),
            Name = contactUsRequest.Name,
            Address = contactUsRequest.Address,
            Email = contactUsRequest.Email,
            PhoneNumber = contactUsRequest.PhoneNumber,
            Message = contactUsRequest.Message
        };

        // Add the contact request to the database
        _context.ContactUs.Add(contactUs);
        await _context.SaveChangesAsync();

        // Check if the contact request was added successfully
        var createdContactUs = await _context.ContactUs.FindAsync(contactUs.Id);
        if (createdContactUs == null)
        {
            return Result<string>.Fail("Failed to create contact request");
        }

        return new Result<string>
        {
            Success = true,
            Message = "Contact request created successfully",
            Data = contactUs.Id.ToString()
        };
    }
}