using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Order.Queries.GetById;

public class GetByIdOrderQueryHandler
 : IRequestHandler<GetByIdOrderQuery, Response<OrderResponseDto>>
    {
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByIdOrderQuery> _validator;

    public GetByIdOrderQueryHandler
    (
        IOrderService orderService,
        IMapper mapper,
        IValidator<GetByIdOrderQuery> validator
    )
    {
        _orderService = orderService;
        _mapper = mapper;
        _validator = validator;
    }


    public async Task<Response<OrderResponseDto>> Handle
    (
        GetByIdOrderQuery request,
        CancellationToken ct
    )
    {
        
        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<OrderResponseDto>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList(),
                Data = null
            };
        }
        
        var orderResponse = await _orderService
            .GetByIdAsync(request.OrderId, ct);


        if (!orderResponse.Succeeded || orderResponse.Data is null)
            return new Response<OrderResponseDto>(
                "Order not found."
            );


        var order = _mapper
            .Map<OrderResponseDto>(orderResponse.Data);


        return new Response
            <OrderResponseDto>(order);
    }
}