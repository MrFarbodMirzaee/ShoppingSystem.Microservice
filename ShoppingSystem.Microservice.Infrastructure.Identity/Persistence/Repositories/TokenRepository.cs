using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Options;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Services;

namespace ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Repositories;

public class TokenRepository : ITokenService
{
    private readonly JwtOptions _jwt;
    private readonly ILogger<TokenRepository> _logger;

    public TokenRepository(
        IOptions<JwtOptions> jwtOptions,
        ILogger<TokenRepository> logger)
    {
        _jwt = jwtOptions.Value;
        _logger = logger;
    }

    public Task<string> CreateTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new("firstName", user.FirstName ?? ""),
            new("lastName", user.LastName ?? "")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwt.Key));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.ExpirationMinutes),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        _logger.LogDebug("JWT token created for user {UserId}", user.Id);

        return Task.FromResult(jwt);
    }

    public Task<string> GenerateRefreshToken()
    {
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        _logger.LogDebug("Refresh token generated");

        return Task.FromResult(refreshToken);
    }
}