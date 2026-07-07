using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.Payment;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.Refund;

public class RefundPaymentCommandHandler
    : IRequestHandler<RefundPaymentCommand, Response<bool>>
{
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IMediator _mediator;
    private readonly ILogger<RefundPaymentCommandHandler> _logger;

    public RefundPaymentCommandHandler
    (
        IPaymentService paymentService,
        IUnitOfWorkBase unitOfWork,
        IMediator mediator,
        ILogger<RefundPaymentCommandHandler> logger
    )
    {
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        RefundPaymentCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting payment refund process. PaymentId: {PaymentId}, UserId: {UserId}.",
            request.PaymentId,
            request.UserId);

        var paymentResponse = await _paymentService
            .GetByIdAsync(request.PaymentId, ct);

        if (!paymentResponse.Succeeded || paymentResponse.Data is null)
        {
            _logger.LogWarning(
                "Payment refund failed. PaymentId {PaymentId} was not found.",
                request.PaymentId);

            return new Response<bool>(
                false,
                "Payment not found.");
        }

        var payment = paymentResponse.Data;

        payment.Refund();

        var result = await _paymentService
            .UpdateAsync(payment, ct);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "Payment refund update failed. PaymentId: {PaymentId}. Reason: {Reason}",
                payment.Id,
                result.Message);

            return new Response<bool>(
                result.Message);
        }

        await _unitOfWork.SaveAsync(ct);

        await _mediator.Publish(
            new PaymentRefundedEvent(
                payment.Id,
                request.UserId,
                payment.Amount,
                request.Message,
                payment.OrderId),
            ct);

        _logger.LogInformation(
            "Payment refunded successfully. PaymentId: {PaymentId}," +
            " OrderId: {OrderId}," +
            " UserId: {UserId}" +
            ", Currency: {Currency}" +
            ". PaymentRefundedEvent published.",
            payment.Id,
            payment.OrderId,
            request.UserId,
            payment.Amount.Currency);

        return new Response<bool>(true);
    }
}