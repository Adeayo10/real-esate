using Microsoft.EntityFrameworkCore;
using server_real_estate.Model;
using server_real_estate.Database;
using Microsoft.AspNetCore.Identity;

namespace server_real_estate.Services;

public interface IUserService
{
    Task<Result<User>> GetUserByIdAsync(string id);
    Task<Result<List<User>>> GetAllUsersAsync();
    Task<Result<User>> CreateUserAsync(RegisterRequest user);
    Task<Result<bool>> UpdateUserAsync(string id, RegisterRequest updatedUser);
    Task<Result<bool>> DeleteUserAsync(string id);
    // Task LogoutUserAsync(string userId);
}

public class UserService(
    IRealEstatateDbContext context,
    ILogger<UserService> logger,
    UserManager<User> userManager,
    ITokenService tokenService) : IUserService
{
    private readonly IRealEstatateDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly ILogger<UserService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly ITokenService _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));

    public async Task<Result<User>> GetUserByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Result<User>.Fail("User ID cannot be null or empty");

        if (!Guid.TryParse(id, out _))
            return Result<User>.Fail("Invalid User ID format");

        try
        {
            var user = await _context.Users.FindAsync(id);
            return user == null
                ? Result<User>.Fail("User not found")
                : Result<User>.Ok(user, "User retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user by ID");
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
            _logger.LogError(ex, "Error retrieving all users");
            return Result<List<User>>.Fail($"Error retrieving users: {ex.Message}");
        }
    }

    public async Task<Result<User>> CreateUserAsync(RegisterRequest user)
    {
        if (user == null)
            return Result<User>.Fail("User data cannot be null");

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

            await _context.Users.AddAsync(newUser);
            _logger.LogInformation("Creating user: {FirstName} {LastName}", newUser.FirstName, newUser.LastName);
            await _context.SaveChangesAsync();

            return Result<User>.Ok(newUser, "User created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return Result<User>.Fail($"Error creating user: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateUserAsync(string id, RegisterRequest updatedUser)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Result<bool>.Fail("User ID cannot be null or empty");

        if (updatedUser == null)
            return Result<bool>.Fail("Updated user data cannot be null");

        try
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return Result<bool>.Fail("User not found");

            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;
            existingUser.Password = updatedUser.Password;
            existingUser.Role = updatedUser.Role;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true, "User updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user");
            return Result<bool>.Fail($"Error updating user: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteUserAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Result<bool>.Fail("User ID cannot be null or empty");

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
            _logger.LogError(ex, "Error deleting user");
            return Result<bool>.Fail($"Error deleting user: {ex.Message}");
        }
    }

    // public async Task LogoutUserAsync(string userId)
    // {
    //     if (string.IsNullOrWhiteSpace(userId))
    //     {
    //         _logger.LogWarning("User ID cannot be null or empty for logout");
    //         return;
    //     }

    //     try
    //     {
    //         var user = await _userManager.FindByIdAsync(userId);
    //         if (user == null)
    //         {
    //             _logger.LogWarning("User not found for logout: {UserId}", userId);
    //             return;
    //         }

    //         user.TokenVersion++;
    //         await _userManager.UpdateAsync(user);
    //         _logger.LogInformation("User {UserId} logged out successfully", userId);

    //         await _tokenService.RevokeTokens(userId);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error logging out user: {UserId}", userId);
    //     }
    // }
}
