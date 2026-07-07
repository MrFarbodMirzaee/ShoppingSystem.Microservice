using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetByTransactionId;

public class GetByTransactionIdPaymentQueryHandler
    : IRequestHandler<GetByTransactionIdPaymentQuery, Response<PaymentResponseDto>>
{
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByTransactionIdPaymentQuery> _validator;

    public GetByTransactionIdPaymentQueryHandler
    (
        IPaymentService paymentService,
        IMapper mapper,
        IValidator<GetByTransactionIdPaymentQuery> validator
    )
    {
        _paymentService = paymentService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<PaymentResponseDto>> Handle
    (
        GetByTransactionIdPaymentQuery request,
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
            .GetByTransactionIdAsync(request.TransactionId, ct);

        if (!paymentResponse.Succeeded || paymentResponse.Data is null)
            return new Response<PaymentResponseDto>(
                "Payment not found with this transaction id."
            );

        var payment = _mapper
            .Map<PaymentResponseDto>(paymentResponse.Data);

        return new Response
        <PaymentResponseDto>(payment);
    }
}