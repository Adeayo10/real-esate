using Microsoft.EntityFrameworkCore;
using server_real_estate.Model;
using server_real_estate.Database;

namespace server_real_estate.Services;
public class UserService : IUserService
{
    private readonly IRealEstatateDbContext _context;
    private readonly ILogger<UserService> _logger;
    public UserService(IRealEstatateDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<Result<User>> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            return user == null
                ? Result<User>.Fail("User not found")
                : Result<User>.Ok(user, "User retrieved successfully");
        }
        catch (Exception ex)
        {
            return Result<User>.Fail($"Error retrieving user: {ex.Message}");
        }
    }

    public async Task<Result<List<User>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _context.Users.ToListAsync();
            return Result<List<User>>.Ok(users, "Users retrieved successfully");
        }
        catch (Exception ex)
        {
            return Result<List<User>>.Fail($"Error retrieving users: {ex.Message}");
        }
    }

    public async Task<Result<User>> CreateUserAsync(RegisterRequest user)
    {
        try
        {
            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role
            };
            _context.Users.Add(newUser);
            _logger.LogInformation($"Creating user: {newUser.FirstName} {newUser.LastName}");
            await _context.SaveChangesAsync();
            return Result<User>.Ok(newUser, "User created successfully");
        }
        catch (Exception ex)
        {
            return Result<User>.Fail($"Error creating user: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateUserAsync(Guid id, RegisterRequest updatedUser)
    {
        try
        {

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return Result<bool>.Fail("User not found");

            _context.Entry(existingUser).CurrentValues.SetValues(updatedUser);
            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true, "User updated successfully");
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Error updating user: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteUserAsync(Guid id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return Result<bool>.Fail("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true, "User deleted successfully");
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Error deleting user: {ex.Message}");
        }
    }
}

public interface IUserService
{
    Task<Result<User>> GetUserByIdAsync(Guid id);
    Task<Result<List<User>>> GetAllUsersAsync();
    Task<Result<User>> CreateUserAsync(RegisterRequest user);
    Task<Result<bool>> UpdateUserAsync(Guid id, RegisterRequest updatedUser);
    Task<Result<bool>> DeleteUserAsync(Guid id);
}