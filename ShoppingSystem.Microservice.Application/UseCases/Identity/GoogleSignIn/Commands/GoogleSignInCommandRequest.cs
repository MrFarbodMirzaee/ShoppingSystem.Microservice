using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.GoogleSignIn.Commands;

public record GoogleSignInCommandRequest(
    string IdToken
) : IRequest<Response<AuthResponse>>;