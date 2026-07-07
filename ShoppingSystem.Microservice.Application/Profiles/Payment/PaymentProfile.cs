using AutoMapper;
using ShoppingSystem.Microservice.Application.Dtos;

namespace ShoppingSystem.Microservice.Application.Profiles.Payment;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        #region Queries
        CreateMap<Domain.Aggregates.Payment.Payment, PaymentResponseDto>()
            .ForCtorParam(nameof(PaymentResponseDto.Id),
                opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(PaymentResponseDto.OrderId),
                opt => opt.MapFrom(src => src.OrderId))
            .ForCtorParam(nameof(PaymentResponseDto.Amount),
                opt => opt.MapFrom(src => src.Amount.Amount))
            .ForCtorParam(nameof(PaymentResponseDto.Currency),
                opt => opt.MapFrom(src => src.Amount.Currency))
            .ForCtorParam(nameof(PaymentResponseDto.TransactionId),
                opt => opt.MapFrom(src => src.TransactionId == null
                    ? null
                    : src.TransactionId.Value))
            .ForCtorParam(nameof(PaymentResponseDto.Status),
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForCtorParam(nameof(PaymentResponseDto.PaidAt),
                opt => opt.MapFrom(src => src.PaidAt));
        #endregion
    }
}