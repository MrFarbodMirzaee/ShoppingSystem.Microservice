using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Cart.Queries.GetByCustomerId;

public class GetByCustomerIdCartQueryHandler
    : IRequestHandler<GetByCustomerIdCartQuery, Response<CartResponseDto>>
{
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByCustomerIdCartQuery> _validator;

    public GetByCustomerIdCartQueryHandler
    (
        ICartService cartService,
        IMapper mapper,
        IValidator<GetByCustomerIdCartQuery> validator
    )
    {
        _cartService = cartService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<CartResponseDto>> Handle
    (
        GetByCustomerIdCartQuery request,
        CancellationToken ct
    )
    {
        
        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<CartResponseDto>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList(),
                Data = null
            };
        }
        
        var cartResponse = await _cartService
            .GetByCustomerIdAsync(request.CustomerId, ct);

        if (!cartResponse.Succeeded || cartResponse.Data is null)
            return new Response<CartResponseDto>
                ("Cart not found.");

        var cart = _mapper
            .Map<CartResponseDto>(cartResponse.Data);

        return new Response
        <CartResponseDto>(cart);
    }
}