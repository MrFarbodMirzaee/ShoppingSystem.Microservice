using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.RefreshToken.Command;

public record RefreshTokenRequestCommand(
    string RefreshToken
) : IRequest<Response<AuthResponse>>;