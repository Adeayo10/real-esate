using Microsoft.AspNetCore.Mvc;
using server_real_estate.Model;
using server_real_estate.Services;

namespace server_real_estate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController(IContactService contactService) : ControllerBase
{
    private readonly IContactService _contactService = contactService;

    [HttpPost("contact-us")]
    [ProducesResponseType(typeof(string), 200, "application/json")]
    [ProducesResponseType(typeof(string), 400, "application/json")]
    public async Task<IActionResult> CreateContactUs([FromBody] ContactUsRequest contactUsRequest)
    {
        if (contactUsRequest == null)
        {
            return BadRequest(new { Message = "Contact request cannot be null." });
        }

        var result = await _contactService.CreateContactUsAsync(contactUsRequest);

        if (result.Success)
        {
            return Ok(new { Message = result.Message, Data = result.Data });
        }
        else
        {
            return BadRequest(new { Message = result.Message });
        }
    }
}