namespace ShoppingSystem.Microservice.Application.Dtos;

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresInSeconds,
    string Email,
    string FirstName,
    bool IsNewUser
);