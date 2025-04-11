using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server_real_estate.Model;
using server_real_estate.Services;
using server_real_estate.Database;

namespace server_real_estate.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController(SignInManager<User>  signInManager, UserManager<User> userManager, ITokenService tokenService, IRealEstatateDbContext dbContext, ILogger<AuthController> logger) : ControllerBase
{
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IRealEstatateDbContext _dbContext = dbContext;
    private readonly ILogger<AuthController> _logger = logger;

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerRequest">The registration request data.</param>
    /// <returns>Returns 200 if registration is successful, 400 if there are validation errors.</returns>
    /// <response code="200">User registered successfully.</response>
    /// <response code="400">Invalid registration data.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(SuccessResponse), 200, "application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 400, "application/json")]
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
            return Conflict(new ErrorResponse { Message = "Email is already in use." });
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = registerRequest.Email,
            Email = registerRequest.Email,
            FirstName = registerRequest.FirstName?.Trim(),
            LastName = registerRequest.LastName?.Trim(),
            Role = string.IsNullOrWhiteSpace(registerRequest.Role) ? "User" : registerRequest.Role.Trim()
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

/// <summary>
/// Logs in a user.
/// </summary>
/// <param name="useCookies">Set to true to use cookie-based authentication.</param>
/// <param name="useSessionCookies">Set to true to use session cookies (valid only for the browser session).</param>
/// <param name="loginRequest">The login request payload.</param>
/// <returns>Returns 200 if login is successful, 400 for validation errors, and 401 for unauthorized access.</returns>
/// <response code="200">Login successful.</response>
/// <response code="400">Invalid login data.</response>
/// <response code="401">Unauthorized access.</response>
[HttpPost("login")]
[ProducesResponseType(typeof(SuccessResponse), 200, "application/json")]
[ProducesResponseType(typeof(ErrorResponse), 400, "application/json")]
[ProducesResponseType(typeof(ErrorResponse), 401, "application/json")]
public async Task<IActionResult> Login(
    [FromQuery] bool useCookies,
    [FromQuery] bool useSessionCookies,
    [FromBody] LoginRequest loginRequest)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(new ErrorResponse { Message = "Invalid login data." });
    }

    var user = await _userManager.FindByEmailAsync(loginRequest.Email);
    if (user == null)
    {
        return Unauthorized(new ErrorResponse { Message = "Invalid email or password." });
    }

    var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, loginRequest.RememberMe, lockoutOnFailure: true);
    if (!result.Succeeded)
    {
        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return Unauthorized(new ErrorResponse { Message = "Account is locked out." });
        }

        return Unauthorized(new ErrorResponse { Message = "Invalid email or password." });
    }

    // If using cookies
    if (useCookies)
    {
        // Use session cookie if specified, otherwise use persistent cookie
        await _signInManager.SignInAsync(user, isPersistent: !useSessionCookies);
    }

    // If using Bearer tokens
    if (loginRequest.UseBearerToken)
    {
        var tokenResult = _tokenService.CreateToken(user.Id.ToString(), user.Email, user.Role);

        return Ok(new SuccessResponse
        {
            Token = tokenResult.Data,
            UserId = user.Id.ToString(),
            Message = "Login successful.",
            IsSuccessful = true,
            StatusCode = "200"
        });
    }

    return Ok(new SuccessResponse
    {
        Message = "Login successful.",
        IsSuccessful = true,
        StatusCode = "200"
    });
}
        
}
