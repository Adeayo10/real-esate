using Microsoft.AspNetCore.Mvc;
using server_real_estate.Model;
using server_real_estate.Services;

namespace server_real_estate.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ListController : ControllerBase
{
    private readonly IListService _listService;
 
    public ListController(IListService listService)
    {
        _listService = listService;
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Property>>> GetAllProperties([FromQuery] int pageNumber,[FromQuery] int pageSize)
    {
        try
        {
            var properties = await _listService.GetAllPropertiesAsync(pageNumber,pageSize);
            return Ok(properties);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Property>> GetPropertyById(int id)
    {
        try
        {
            var property = await _listService.GetPropertyByIdAsync(id);
            if (property == null)
                return NotFound($"Property with ID {id} not found.");
            return Ok(property);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Property>> CreateProperty(Property property)
    {
        try
        {
            var newProperty = await _listService.CreatePropertyAsync(property);
            return CreatedAtAction(nameof(GetPropertyById), new { id = newProperty.Data.Id }, newProperty.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Property>> UpdateProperty(int id, Property updatedProperty)
    {
        try
        {
            var result = await _listService.UpdatePropertyAsync(id, updatedProperty);
            if (!result.Data)
            {
                return NotFound($"Property with ID {id} not found.");
            }
            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteProperty(int id)
    {
        try
        {
            var result = await _listService.DeletePropertyAsync(id);
            if (!result.Data)
            {
                return NotFound($"Property with ID {id} not found.");
            }
            return Ok("Property Deleted");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Property>>> SearchProperties(string search)
    {
        try
        {
            var properties = await _listService.SearchPropertyAsync(search);
            return Ok(properties);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("sort")]
    public async Task<IActionResult> sortProperty(string sortValue)
    {
        try
        {
            var properties = await _listService.sortProperty(sortValue);
            return Ok(properties);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("searchbytype")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Property>>> SearchByType(string Type)
    {
        try{
            var properties = await _listService.SearchByType(Type);
            return Ok(properties);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("fileterproperty")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<Property>>> FilterProperty(string type,string mode)
    {
        try{
            var properties = await _listService.FilterProperty(type,mode);
            return Ok(properties);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
