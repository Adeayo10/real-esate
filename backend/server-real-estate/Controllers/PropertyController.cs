using Microsoft.AspNetCore.Mvc;
using server_real_estate.Data;
using server_real_estate.Model;

namespace realEstate.Controllers;
[ApiController]
[Route("[controller]")]
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
