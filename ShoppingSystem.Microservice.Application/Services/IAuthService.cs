using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.Services;

public interface IAuthService
{
    
    Task<Response<AuthResponse>> SignUpAsync
        (string email
        , string password
        , string? firstName
        , string? lastName
        , CancellationToken ct);

    Task<Response<AuthResponse>> SignInAsync
    (string email, string password, CancellationToken ct);
    
    Task<Response<AuthResponse>> GoogleLoginAsync
        (
        string idToken
        ,CancellationToken ct);

    Task<Response<AuthResponse>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken ct);

    Task<Response<bool>> RevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken ct);
}