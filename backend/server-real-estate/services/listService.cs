using Microsoft.EntityFrameworkCore;
using server_real_estate.Model;
using server_real_estate.Data;
using static server_real_estate.Services.ListService;
using server_real_estate.Models.Property;

namespace server_real_estate.Services;

public class ListService : IListService
{
    private readonly IRealEstatateDbContext _context;
    private readonly ILogger<ListService> _logger;
    public ListService(IRealEstatateDbContext context, ILogger<ListService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<Result<List<Property>>> GetAllPropertiesAsync(int pageNumber,int pageSize)
    {
        try
        {
            int totalProperties = await _context.Properties.CountAsync();
            var properties = await _context.Properties.OrderBy(x=>x.Id).Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
            var result = new pageResult<Property>
            {
                items = properties,
                totalItems = totalProperties
            };
            return Result<List<Property>>.Ok(properties, "Properties retrieved successfully.");
        }
        catch (Exception ex)
        {
            return Result<List<Property>>.Fail($"Error retrieving properties: {ex.Message}");
        }
    }
    public async Task<Result<Property>> GetPropertyByIdAsync(int id)
    {
        try
        {
            var property = await _context.Properties.FirstOrDefaultAsync(p => p.Id == id);
            if (property == null)
            {
                return Result<Property>.Fail("Property not found.");
            }
            return Result<Property>.Ok(property, "Property retrieved successfully.");
        }
        catch (Exception ex)
        {
            return Result<Property>.Fail($"Error retrieving property: {ex.Message}");
        }
    }
    public async Task<Result<Property>> CreatePropertyAsync(Property property)
    {
        try
        {
            await _context.Properties.AddAsync(property);
            await _context.SaveChangesAsync();
            return Result<Property>.Ok(property, "Property created successfully.");
        }
        catch (Exception ex)
        {
            return Result<Property>.Fail($"Error creating property: {ex.Message}");
        }
    }
    public async Task<Result<bool>> UpdatePropertyAsync(int id, Property updatedProperty)
    {
        try
        {
            var property = await _context.Properties.FirstOrDefaultAsync(p => p.Id == id);
            if (property == null)
            {
                return Result<bool>.Fail("Property not found.");
            }
            property.Name = updatedProperty.Name;
            property.Address = updatedProperty.Address;
            property.Price = updatedProperty.Price;
            _context.Properties.Update(property);
            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true, "Property updated successfully.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Error updating property: {ex.Message}");
        }
    }
    public async Task<Result<bool>> DeletePropertyAsync(int id)
    {
        try
        {
            var property = await _context.Properties.FirstOrDefaultAsync(p => p.Id == id);
            if (property == null)
            {
                return Result<bool>.Fail("Property not found.");
            }
            _context.Properties.Remove(property);
            _context.SaveChanges();
            return Result<bool>.Ok(true, "Property deleted successfully.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Error deleting property: {ex.Message}");

        }
    }
    public async Task<Result<List<Property>>> SearchPropertyAsync(string searchTerm)
    {
        try
        {
            var properties = await _context.Properties
                .Where(p => p.Name.Contains(searchTerm) || p.Address.Contains(searchTerm))
                .ToListAsync();
            if (properties == null)
            {
                return Result<List<Property>>.Fail("Property not found.");
            }
            return Result<List<Property>>.Ok(properties, "Properties retrieved successfully.");
        }
        catch (Exception ex)
        {
            return Result<List<Property>>.Fail($"Error searching properties: {ex.Message}");
        }
    }

    public async Task<Result<List<Property>>> sortProperty(string sortValue)
    {
        try
        {
            var properties = await _context.Properties.ToListAsync();
            if (sortValue == "asc")
            {
                properties = properties.OrderBy(p => p.Price).ToList();
            }
            else if (sortValue == "desc")
            {
                properties = properties.OrderByDescending(p => p.Price).ToList();
            }
            return Result<List<Property>>.Ok(properties, "Properties sorted successfully.");
        }
        catch (Exception ex)
        {
            return Result<List<Property>>.Fail($"Error sorting properties: {ex.Message}");
        }
    }
}


public interface IListService
{
    Task<Result<List<Property>>> GetAllPropertiesAsync(int pageNumber,int pageSize);
    Task<Result<Property>> GetPropertyByIdAsync(int id);
    Task<Result<Property>> CreatePropertyAsync(Property property);
    Task<Result<bool>> UpdatePropertyAsync(int id, Property updatedProperty);
    Task<Result<bool>> DeletePropertyAsync(int id);
    Task<Result<List<Property>>> SearchPropertyAsync(string searchTerm);
    Task<Result<List<Property>>> sortProperty(string sortValue);
}