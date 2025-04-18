using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server_real_estate.Model;
using server_real_estate.Services;
using server_real_estate.Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace server_real_estate.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController(SignInManager<User> signInManager, UserManager<User> userManager, ITokenService tokenService, IRealEstatateDbContext dbContext, ILogger<AuthController> logger) : ControllerBase
{
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IRealEstatateDbContext _dbContext = dbContext;
    private readonly ILogger<AuthController> _logger = logger;


    [HttpPost("register")]
    [ProducesResponseType(typeof(SuccessResponse), 200, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 400, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 409, "application/json")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        if (registerRequest == null)
        {
            return BadRequest(new ErrorResponse { Message = "Registration data cannot be null." });
        }

        if (string.IsNullOrWhiteSpace(registerRequest.Email) || string.IsNullOrWhiteSpace(registerRequest.Password))
        {
            return BadRequest(new ErrorResponse { Message = "Email and password are required." });
        }

        if (!IsValidEmail(registerRequest.Email))
        {
            return BadRequest(new ErrorResponse { Message = "Invalid email format." });
        }

        var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
        if (existingUser != null)
        {
            return Conflict(new ErrorResponse
            {
                Message = "Email is already in use.",
                IsSuccessful = false,
                StatusCode = "409"
            });
        }

        var user = new User
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
            FirstName = registerRequest.FirstName!.Trim(),
            LastName = registerRequest.LastName!.Trim(),
            Role = string.IsNullOrEmpty(registerRequest.Role) ? "User" : registerRequest.Role.Trim(), 
            Address = registerRequest.Address.Trim(),
            PhoneNumber = registerRequest.PhoneNumber?.Trim(),
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new ErrorResponse
            {
                Message = "User registration failed.",
                Errors = errors,
                IsSuccessful = false,
                StatusCode = "400"
            });
        }

        return Ok(new SuccessResponse
        {
            UserId = user.Id.ToString(),
            Message = "User registered successfully.",
            IsSuccessful = true,
            StatusCode = "200"
        });
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }


    [HttpPost("login")]
    [ProducesResponseType(typeof(SuccessResponse), 200, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 400, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 401, "application/json")]
    public async Task<IActionResult> Login(
        [FromQuery] bool useCookies,
        [FromQuery] bool useSessionCookies,
        [FromBody] LoginRequest loginRequest)
    {
        if (loginRequest == null || !ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse { Message = "Invalid login data." });
        }

        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null)
        {
            return BadRequest(new ErrorResponse { Message = "Invalid email or password." });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out for email: {Email}", loginRequest.Email);
                return Unauthorized(new ErrorResponse { Message = "Account is locked out." });
            }

            return BadRequest(new ErrorResponse { Message = "Invalid email or password." });
        }

        // Dynamically configure cookie-based authentication when useCookies is selected
        if (useCookies || useSessionCookies)
        {
            if (loginRequest.UseBearerToken)
            {
                return BadRequest(new ErrorResponse { Message = "Cannot use both cookies and bearer token simultaneously." });
            }

            // Add cookie authentication dynamically
            var cookieAuthScheme = IdentityConstants.ApplicationScheme;
            HttpContext.Response.Cookies.Append(".AspNetCore.Cookies", ""); // Example cookie setup

            await _signInManager.SignInAsync(user, isPersistent: !useSessionCookies);
            _logger.LogInformation("User {UserId} logged in using cookies.", user.Id);
            return Ok(new SuccessResponse
            {
                UserId = user.Id.ToString(),
                Message = "Login successful.",
                IsSuccessful = true,
                StatusCode = "200"
            });
        }

        if (loginRequest.UseBearerToken)
        {
            if (user.TokenVersion != await _dbContext.Users
                .Where(u => u.Id == user.Id)
                .Select(u => u.TokenVersion)
                .FirstOrDefaultAsync())
            {
                return Unauthorized(new ErrorResponse { Message = "Token version mismatch." });
            }

            var tokenResult = await _tokenService.CreateToken(user.Id.ToString(), user.Email!, user.Role!);
            _logger.LogInformation("User {UserId} logged in successfully.", user.Id);
            if (!tokenResult.Success)
            {
                return BadRequest(new ErrorResponse { Message = "Failed to create token." });
            }
            _logger.LogInformation("Token created successfully for user {UserId}.", user.Id);
            return Ok(new SuccessResponse
            {
                TokenType = "Bearer",
                Token = tokenResult.Data,
                RefreshToken = tokenResult.RefreshToken,
                UserId = user.Id.ToString(),
                Message = "Login successful.",
                IsSuccessful = true,
                StatusCode = "200",
                Expires = tokenResult.Expires
            });
        }

        return BadRequest(new ErrorResponse { Message = "Invalid login request." });
    }


    [HttpPost("refresh-token")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(SuccessResponse), 200, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 400, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 401, "application/json")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        if (refreshTokenRequest == null || string.IsNullOrEmpty(refreshTokenRequest.Token) || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken))
        {
            return BadRequest(new ErrorResponse { Message = "Access token and refresh token are required." });
        }

        // Validate the access token
        var validatedAccessToken =  await _tokenService.ValidateToken(refreshTokenRequest.Token);
        _logger.LogInformation("Access token validation result: {Result}, {Message}", validatedAccessToken.Success, validatedAccessToken.Message);
        if (!validatedAccessToken.Success)
        {
            return Unauthorized(new ErrorResponse { Message = "Invalid access token." });
        }

        // Validate the refresh token
        var storedRefreshToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshTokenRequest.RefreshToken && rt.ExpiryDate > DateTime.UtcNow);
        if (storedRefreshToken == null || storedRefreshToken.IsRevoked || storedRefreshToken.ExpiryDate <= DateTime.UtcNow)
        {
            return Unauthorized(new ErrorResponse { Message = "Invalid or expired refresh token." });
        }

        // Retrieve the user associated with the refresh token
        var user = await _userManager.FindByIdAsync(storedRefreshToken.UserId.ToString());
        if (user == null)
        {
            return Unauthorized(new ErrorResponse { Message = "User not found." });
        }

        // Revoke the old refresh token
        storedRefreshToken.IsRevoked = true;
        _dbContext.RefreshTokens.Update(storedRefreshToken);
        await _dbContext.SaveChangesAsync();

        // Generate a new token
        var tokenResult = await _tokenService.CreateToken(user.Id.ToString(), user.Email, user.Role);
        if (!tokenResult.Success)
        {
            return BadRequest(new ErrorResponse { Message = "Failed to create new token." });
        }

        return Ok(new SuccessResponse
        {
            TokenType = "Bearer",
            Token = tokenResult.Data,
            RefreshToken = tokenResult.RefreshToken,
            UserId = user.Id.ToString(),
            Message = "Token refreshed successfully.",
            IsSuccessful = true,
            StatusCode = "200",
            Expires = tokenResult.Expires
        });
    }

    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(void), 204, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 400, "application/json")]
    public async Task<IActionResult> Logout([FromQuery] bool useCookies, [FromBody] string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest(new ErrorResponse { Message = "User ID is required." });
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return BadRequest(new ErrorResponse { Message = "User not found." });
        }

        // Handle session cookies logout
        if (useCookies)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User {UserId} logged out using session cookies.", userId);
        }

        // Handle JWT token logout
        // user.TokenVersion++;
        // await _userManager.UpdateAsync(user);
        await _tokenService.RevokeTokens(userId);
        _logger.LogInformation("User {UserId} logged out and tokens invalidated.", userId);

        return Ok(new SuccessResponse
        {
            Message = "User logged out successfully.",
            IsSuccessful = true,
            StatusCode = "200"
        });
    }

}

//{create Role auth}
