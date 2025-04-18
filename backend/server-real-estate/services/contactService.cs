using Microsoft.EntityFrameworkCore;
using server_real_estate.Model;
using server_real_estate.Database;

namespace server_real_estate.Services;

public interface IContactService
{
    Task<Result<ContactUsRequest>> CreateContactUsAsync(ContactUsRequest contactUsRequest);
}

public class ContactService(IRealEstatateDbContext context) : IUserService
{
    private readonly IRealEstatateDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    public async Task<Result<ContactUsRequest>> CreateContactUsAsync(ContactUsRequest contactUsRequest)
    {
        if (contactUsRequest == null)
            return Result<contactUsRequest>.Fail("Contact Request cannot be null");

        try
        {
            var newContactRequest = new ContactUs
            {
                Name = contactUsRequest.Name,
                Address = contactUsRequest.Address,
                Email = contactUsRequest.Email,
                Phone = contactUsRequest.Phone,
                Message = contactUsRequest.Message
            };
            await _context.ContactUs.AddAsync(newContactRequest);
            await _context.SaveChangesAsync();

            return Result<ContactUsRequest>.Ok(newContactRequest, "Contact details submitted");
        }
        catch (Exception ex)
        {
            return Result<ContactUsRequest>.Fail($"Error submitting contact details: {ex.Message}");
        }
    }
}