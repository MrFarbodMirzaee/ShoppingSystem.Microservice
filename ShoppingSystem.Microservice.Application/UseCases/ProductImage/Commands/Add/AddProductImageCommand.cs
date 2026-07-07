using MediatR;
using Microsoft.AspNetCore.Http;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.ProductImage.Commands.Add;

public sealed record AddProductImageCommand(
    Guid ProductId,
    IFormFile File,
    bool IsMain,
    Guid UserId
) : IRequest<Response<bool>>;