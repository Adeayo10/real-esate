using Microsoft.EntityFrameworkCore;
using server_real_estate.Model;
using server_real_estate.Data;
using static server_real_estate.Services.ListService;

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
    public async Task<Result<List<Property>>> GetAllPropertiesAsync()
    {
        try
        {
            var properties = await _context.Properties.ToListAsync();
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
}


public interface IListService
{
    Task<Result<List<Property>>> GetAllPropertiesAsync();
    Task<Result<Property>> GetPropertyByIdAsync(int id);
    Task<Result<Property>> CreatePropertyAsync(Property property);
    Task<Result<bool>> UpdatePropertyAsync(int id, Property updatedProperty);
    Task<Result<bool>> DeletePropertyAsync(int id);
    Task<Result<List<Property>>> SearchPropertyAsync(string searchTerm);
}