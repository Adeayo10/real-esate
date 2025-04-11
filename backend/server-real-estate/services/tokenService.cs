using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using server_real_estate.Model;

namespace server_real_estate.Services;

public interface ITokenService
{
    Result<string> CreateToken(string userId, string email, string role);
    Result<bool> ValidateToken(string token);
    Result<string> RefreshToken(string token);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = _configuration["JwtSettings:key"]!;
    }

    public Result<string> CreateToken(string userId, string email, string role)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Result<string>.Ok(tokenHandler.WriteToken(token), "Token created successfully");
            //{return the expires times}
           
        }
        catch (Exception ex)
        {
            return Result<string>.Fail("Failed to create token: " + ex.Message);
        }
    }

    public Result<bool> ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return Result<bool>.Fail("Token is null or empty");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return Result<bool>.Ok(true, "Token is valid");
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail("Token validation failed: " + ex.Message);
        }
    }

    public Result<string> RefreshToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return Result<string>.Fail("Token is null or empty");

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var role = principal.FindFirst(ClaimTypes.Role)?.Value;

            if (userId == null || email == null || role == null)
                return Result<string>.Fail("Invalid token");

            return CreateToken(userId, email, role);
        }
        catch (Exception ex)
        {
            return Result<string>.Fail("Failed to refresh token: " + ex.Message);
        }
    }
}
