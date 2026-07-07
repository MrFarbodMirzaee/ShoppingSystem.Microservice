using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Events.Payment;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.UseCases.Payment.Commands.Fail;

public class FailPaymentCommandHandler
    : IRequestHandler<FailPaymentCommand, Response<bool>>
{
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWorkBase _unitOfWork;
    private readonly IMediator _mediator;
    private readonly ILogger<FailPaymentCommandHandler> _logger;

    public FailPaymentCommandHandler(
        IPaymentService paymentService,
        IUnitOfWorkBase unitOfWork,
        IMediator mediator,
        ILogger<FailPaymentCommandHandler> logger)
    {
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Response<bool>> Handle(
        FailPaymentCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation(
            "Starting payment failure process. PaymentId: {PaymentId}, UserId: {UserId}.",
            request.PaymentId,
            request.userId);

        var paymentResponse = await _paymentService
            .GetByIdAsync(request.PaymentId, ct);

        if (!paymentResponse.Succeeded || paymentResponse.Data is null)
        {
            _logger.LogWarning(
                "Payment failure process failed. PaymentId {PaymentId} was not found.",
                request.PaymentId);

            return new Response<bool>(
                false,
                "Payment not found.");
        }

        var payment = paymentResponse.Data;

        payment.Fail();

        var result = await _paymentService
            .UpdateAsync(payment, ct);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "Payment status update failed. PaymentId: {PaymentId}. Reason: {Reason}",
                payment.Id,
                result.Message);

            return new Response<bool>(
                result.Message);
        }

        await _unitOfWork.SaveAsync(ct);

        await _mediator.Publish(
            new PaymentFailedEvent(
                payment.Id,
                payment.OrderId,
                request.userId,
                request.Reason),
            ct);

        _logger.LogInformation(
            "Payment failed successfully. PaymentId: {PaymentId}, OrderId: {OrderId}, UserId: {UserId}, Reason: {Reason}. PaymentFailedEvent published.",
            payment.Id,
            payment.OrderId,
            request.userId,
            request.Reason);

        return new Response<bool>(true);
    }
}