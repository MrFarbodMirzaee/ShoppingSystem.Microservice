using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.SignUp.Commands;

public record SignUpCommandRequest(
    string Email,
    string Password,
    string? FirstName,
    string? LastName
) : IRequest<Response<AuthResponse>>;