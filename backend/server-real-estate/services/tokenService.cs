using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using server_real_estate.Model;
using server_real_estate.Database;
using Microsoft.AspNetCore.Identity;

namespace server_real_estate.Services;
public interface ITokenService
{
    Task<Result<string>> CreateToken(string userId, string email, string role);
    Task<Result<bool>> ValidateToken(string token);
    Task RevokeTokens(string userId);
}

public class TokenService(
    IConfiguration configuration,
    IRealEstatateDbContext dbContext,
    ILogger<TokenService> logger,
    UserManager<User> userManager,
    TokenValidationParameters _tokenValidationParameters) : ITokenService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IRealEstatateDbContext _dbContext = dbContext;
    private readonly ILogger<TokenService> _logger = logger;
    private readonly UserManager<User> _userManager = userManager;
    private readonly TokenValidationParameters _tokenValidationParameters = _tokenValidationParameters;

    public async Task<Result<string>> CreateToken(string userId, string email, string role)
    {
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("User ID is null or empty.");
            return Result<string>.Fail("User ID is null or empty");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result<string>.Fail("User not found");
        }

        var claims = GenerateClaims(user, email, role);
        var token = GenerateJwtToken(claims);
        var refreshToken = await CreateRefreshToken(userId);

        return new Result<string>
        {
            Success = true,
            Message = "Token created successfully",
            Data = new JwtSecurityTokenHandler().WriteToken(token),
            Expires = token.ValidTo,
            RefreshToken = refreshToken
        };
    }

    public async Task<Result<bool>> ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("Token is null or empty.");
            return Result<bool>.Fail("Token is null or empty");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        // var key = Encoding.ASCII.GetBytes(_secretKey);
        try
        {
            _logger.LogDebug("Starting token validation.");
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.LogWarning("Invalid token format or signing algorithm.");
                return Result<bool>.Fail("Invalid token format or signing algorithm");
            }

            var userIdClaim = principal.FindFirst("userId")?.Value;
            _logger.LogInformation("the claim value userId: {UserId}", userIdClaim);
            var tokenVersionClaim = principal.FindFirst("tokenVersion")?.Value;
            _logger.LogInformation("Extracted userId: {tokenVersionClaim}", tokenVersionClaim);


            var user = await _userManager.FindByIdAsync(userIdClaim);
            if (user == null)
            {
                _logger.LogWarning("User not found for userId: {UserId}", userIdClaim);
                return Result<bool>.Fail("Token is invalid or outdated");
            }

            if (tokenVersionClaim == null)
            {
                _logger.LogWarning("Token version mismatch for userId: {UserId}", userIdClaim);
                return Result<bool>.Fail("Token is invalid or outdated");
            }

            _logger.LogDebug("Token is valid for userId: {UserId}", userIdClaim);
            return Result<bool>.Ok(true, "Token is valid");
        }
        catch (SecurityTokenExpiredException)
        {
            _logger.LogWarning("Token has expired.");
            return Result<bool>.Fail("Token has expired");
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError("Token validation failed: {Error}", ex.Message);
            return Result<bool>.Fail("Token validation failed: " + ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("An unexpected error occurred during token validation: {Error}", ex.Message);
            return Result<bool>.Fail("An unexpected error occurred: " + ex.Message);
        }
    }

    public async Task RevokeTokens(string userId)
    {
        try
        {
            var refreshTokens = _dbContext.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToList();

            foreach (var token in refreshTokens)
            {
                token.IsRevoked = true;
                _dbContext.RefreshTokens.Update(token);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.TokenVersion++;
                await _userManager.UpdateAsync(user);
            }

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("All tokens for user {UserId} have been revoked.", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while revoking tokens for user {UserId}: {Error}", userId, ex.Message);
            throw;
        }
    }

    private IEnumerable<Claim> GenerateClaims(User user, string email, string role)
    {
        var now = DateTimeOffset.UtcNow;
        return
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Aud, _tokenValidationParameters.ValidAudience!),
            new Claim(JwtRegisteredClaimNames.Iss, _tokenValidationParameters.ValidIssuer!),
            new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Nbf, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Exp, now.AddMinutes(5).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim("tokenVersion", user.TokenVersion.ToString()),
            new Claim("userId", user.Id.ToString())
        ];
    }

    private JwtSecurityToken GenerateJwtToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: _tokenValidationParameters.ValidIssuer,
            audience: _tokenValidationParameters.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: creds

        );
    }


    private static string GenerateRefreshToken()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, 32).Select(s => s[new Random().Next(s.Length)]).ToArray());
    }

    private async Task<string> CreateRefreshToken(string userId)
    {
        var refreshToken = GenerateRefreshToken();
        var expiryDate = DateTime.UtcNow.AddDays(7);

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = refreshToken,
            ExpiryDate = expiryDate,
            IsRevoked = false
        };

        await SaveRefreshToken(newRefreshToken);
        return refreshToken;
    }

    private async Task SaveRefreshToken(RefreshToken refreshToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();
    }
}
