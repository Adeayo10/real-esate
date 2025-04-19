using Microsoft.AspNetCore.Mvc;
using server_real_estate.Model;
using server_real_estate.Services;
using server_real_estate.Database;

namespace server_real_estate.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactUsRequest>> SubmitRequest(ContactUsRequest contactRequest)
    {
        try
        {
            var newRequest = await _contactService.CreateContactUsAsync(contactRequest);
            return CreatedAtAction(nameof(GetContactUsByName), new { id = newRequest.Data.Name }, newRequest.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}