using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.SignIn.Commands;

public class SignInCommandRequest : IRequest<Response<AuthResponse>>
{
    public string Email { get; set; } 
    public string Password { get; set; } 
}