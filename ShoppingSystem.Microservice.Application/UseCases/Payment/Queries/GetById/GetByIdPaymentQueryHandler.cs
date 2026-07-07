using AutoMapper;
using FluentValidation;
using MediatR;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Queries.GetById;

public class GetByIdPaymentQueryHandler
    : IRequestHandler<GetByIdPaymentQuery, Response<PaymentResponseDto>>
{
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;
    private readonly IValidator<GetByIdPaymentQuery> _validator;

    public GetByIdPaymentQueryHandler
    (
        IPaymentService paymentService,
        IMapper mapper,
        IValidator<GetByIdPaymentQuery> validator
    )
    {
        _paymentService = paymentService;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Response<PaymentResponseDto>> Handle
    (
        GetByIdPaymentQuery request,
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
            .GetByIdAsync(request.PaymentId, ct);

        if (!paymentResponse.Succeeded || paymentResponse.Data is null)
            return new Response<PaymentResponseDto>
                ("Payment not found.");

        var payment = _mapper
            .Map<PaymentResponseDto>(paymentResponse.Data);

        return new Response
        <PaymentResponseDto>(payment);
    }
}