using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetByOrderId;

public class GetByOrderIdPaymentQueryHandler
    : IRequestHandler<GetByOrderIdPaymentQuery, Response<PaymentResponseDto>>
{
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByOrderIdPaymentQuery> _validator;

    public GetByOrderIdPaymentQueryHandler
    (
        IPaymentService paymentService,
        IMapper mapper,
        IValidator<GetByOrderIdPaymentQuery> validator
    )
    {
        _paymentService = paymentService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<PaymentResponseDto>> Handle
    (
        GetByOrderIdPaymentQuery request,
        CancellationToken ct
    )
    {
        
        var validationResult = await _validator
            .ValidateAsync(request, ct);

        if (!validationResult.IsValid)
        {
            return new Response<PaymentResponseDto>
            {
                Succeeded = false,
                Message = "Validation failed.",
                Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList(),
                Data = null
            };
        }
        
        var paymentResponse = await _paymentService
            .GetByOrderIdAsync(request.OrderId, ct);

        if (!paymentResponse.Succeeded || paymentResponse.Data is null)
            return new Response<PaymentResponseDto>(
                "Payment not found for this order."
            );

        var payment = _mapper
            .Map<PaymentResponseDto>(paymentResponse.Data);

        return new Response<PaymentResponseDto>(payment);
    }
}