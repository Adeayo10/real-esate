using Microsoft.AspNetCore.Mvc;
using server_real_estate.Database;
using server_real_estate.Model;

namespace server_real_estate.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PropertyController : Controller
{
    private readonly RealEstateDbContext _context;

    public PropertyController(RealEstateDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public IEnumerable<Property> GetProperties()
    {
        return _context.Properties.ToList();
    }

    [HttpPost]    
    public IActionResult AddProperty(Property property)
    {
        _context.Properties.Add(property);
        _context.SaveChanges();
        return Ok();
    }
}
