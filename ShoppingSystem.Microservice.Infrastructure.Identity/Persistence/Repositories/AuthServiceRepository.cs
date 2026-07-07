using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;
using ShoppingSystem.Microservice.Infrastructure.Identity.Context;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Services;

namespace ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Repositories;

public class AuthServiceRepository : IAuthService
{
    private readonly IdentityAppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthServiceRepository> _logger;

    public AuthServiceRepository(
        IdentityAppDbContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        ILogger<AuthServiceRepository> logger)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<Response<AuthResponse>> SignUpAsync(
        string email,
        string password,
        string? firstName,
        string? lastName,
        CancellationToken ct)
    {
        _logger.LogInformation("Signup attempt for {Email}", email);

        var existingUser = await _userManager
            .FindByEmailAsync(email);

        if (existingUser is not null)
        {
            _logger.LogWarning("Signup failed - user already exists: {Email}", email);

            return new Response<AuthResponse>
            {
                Succeeded = false,
                Message = "User already exists."
            };
        }

        var user = new ApplicationUser
        {
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName
        };

        var createResult = await _userManager
            .CreateAsync(user, password);

        if (!createResult.Succeeded)
        {
            _logger.LogError(
                "Signup failed for {Email}. Errors: {Errors}",
                email,
                string.Join(", ", createResult.Errors
                    .Select(e => e.Description)));

            return new Response<AuthResponse>
            {
                Succeeded = false,
                Message = string.Join(", ", createResult.Errors
                    .Select(e => e.Description))
            };
        }

        var accessToken = await _tokenService
            .CreateTokenAsync(user);
        
        var refreshToken = await _tokenService
            .GenerateRefreshToken();

        var refreshEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };

        _context.RefreshTokens.Add(refreshEntity);
        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("User registered successfully: {UserId}", user.Id);

        return new Response<AuthResponse>
        {
            Succeeded = true,
            Message = "User registered successfully.",
            Data = new AuthResponse(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                ExpiresInSeconds: 1800,
                Email: email,
                FirstName: firstName,
                IsNewUser: true
            )
        };
    }

    public async Task<Response<AuthResponse>> SignInAsync(
        string email,
        string password,
        CancellationToken ct)
    {
        _logger.LogInformation("Login attempt for {Email}", email);

        var user = await _userManager
            .FindByEmailAsync(email);

        if (user is null)
        {
            _logger.LogWarning("Login failed - user not found: {Email}", email);

            return new Response<AuthResponse>
            {
                Succeeded = false,
                Message = "Invalid email or password."
            };
        }

        var signInResult = await _signInManager
            .CheckPasswordSignInAsync(
            user,
            password,
            lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            _logger.LogWarning("Login failed - invalid password: {Email}", email);

            return new Response<AuthResponse>
            {
                Succeeded = false,
                Message = "Invalid email or password."
            };
        }

        var accessToken = await _tokenService
            .CreateTokenAsync(user);
        
        var refreshToken = await _tokenService
            .GenerateRefreshToken();

        var refreshEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };

        _context.RefreshTokens.Add(refreshEntity);
        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Login successful for {UserId}", user.Id);

        return new Response<AuthResponse>
        {
            Succeeded = true,
            Message = "Login successful.",
            Data = new AuthResponse(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                ExpiresInSeconds: 1800,
                Email: user.Email!,
                FirstName: user.FirstName,
                IsNewUser: false
            )
        };
    }

    public async Task<Response<AuthResponse>> GoogleLoginAsync(
        string idToken,
        CancellationToken ct)
    {
        _logger.LogInformation("Google login attempt");

        var payload = await GoogleJsonWebSignature
            .ValidateAsync(idToken);

        var googleId = payload.Subject;
        var email = payload.Email;
        var firstName = payload.GivenName;
        var lastName = payload.FamilyName;

        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.LogWarning("Google login failed - missing email in token");

            return new Response<AuthResponse>
            {
                Succeeded = false,
                Message = "Google token does not contain email."
            };
        }

        var user = await _userManager
            .FindByEmailAsync(email);

        bool isNewUser = false;

        if (user is null)
        {
            isNewUser = true;

            user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                GoogleId = googleId,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true
            };

            var createResult = await _userManager
                .CreateAsync(user);

            if (!createResult.Succeeded)
            {
                _logger.LogError(
                    "Google signup failed for {Email}: {Errors}",
                    email,
                    string.Join(", ", createResult.Errors.Select(e => e.Description)));

                return new Response<AuthResponse>
                {
                    Succeeded = false,
                    Message = string.Join(", ", createResult.Errors.Select(e => e.Description))
                };
            }

            _logger.LogInformation("New Google user created: {UserId}", user.Id);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(user.GoogleId) &&
                user.GoogleId != googleId)
            {
                _logger.LogWarning("Google account mismatch for {Email}", email);

                return new Response<AuthResponse>
                {
                    Succeeded = false,
                    Message = "Google account mismatch."
                };
            }

            if (string.IsNullOrWhiteSpace(user.GoogleId))
            {
                user.GoogleId = googleId;
                await _userManager.UpdateAsync(user);
            }
        }

        var accessToken = await _tokenService
            .CreateTokenAsync(user);
        
        var refreshToken = await _tokenService
            .GenerateRefreshToken();

        _context.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        });

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Google login successful for {UserId}", user.Id);

        return new Response<AuthResponse>
        {
            Succeeded = true,
            Message = "Google login successful.",
            Data = new AuthResponse(
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                ExpiresInSeconds: 1800,
                Email: email,
                FirstName: firstName,
                IsNewUser: isNewUser
            )
        };
    }

    public async Task<Response<AuthResponse>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken ct)
    {
        _logger.LogInformation("Refresh token attempt");

        var storedToken = await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == refreshToken, ct);

        if (storedToken is null || !storedToken.IsActive)
        {
            _logger.LogWarning("Invalid refresh token attempt");

            return new Response<AuthResponse>
            {
                Succeeded = false,
                Message = "Invalid or expired refresh token."
            };
        }

        storedToken.RevokedAt = DateTime.UtcNow;

        var user = storedToken.User;

        var newAccessToken = await _tokenService
            .CreateTokenAsync(user);
        
        var newRefreshToken = await _tokenService
            .GenerateRefreshToken();

        _context.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = newRefreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        });

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Token refreshed for user {UserId}", user.Id);

        return new Response<AuthResponse>
        {
            Succeeded = true,
            Message = "Token refreshed successfully.",
            Data = new AuthResponse(
                AccessToken: newAccessToken,
                RefreshToken: newRefreshToken,
                ExpiresInSeconds: 1800,
                Email: user.Email!,
                FirstName: user.FirstName,
                IsNewUser: false
            )
        };
    }

    public async Task<Response<bool>> RevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken ct)
    {
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken, ct);

        if (storedToken is null)
        {
            _logger.LogWarning("Revoke failed - refresh token not found");

            return new Response<bool>
            {
                Succeeded = false,
                Data = false,
                Message = "Refresh token not found."
            };
        }

        storedToken.RevokedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation
            ("Refresh token revoked for user {UserId}", storedToken.UserId);

        return new Response<bool>
        {
            Succeeded = true,
            Data = true,
            Message = "Refresh token revoked successfully."
        };
    }
}