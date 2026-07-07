using MediatR;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.RefreshToken.Command.Revoke;

public record RevokeRefreshTokenCommandRequest(
    string RefreshToken
) : IRequest<Response<bool>>;