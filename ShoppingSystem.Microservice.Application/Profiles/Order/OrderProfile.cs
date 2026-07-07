using AutoMapper;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Entities;

namespace ShoppingSystem.Microservice.Application.Profiles.Order;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        #region Queries
        CreateMap<Domain.Aggregates.Order.Order, OrderResponseDto>()
            .ForCtorParam(nameof(OrderResponseDto.Id),
                opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(OrderResponseDto.UserId),
                opt => opt.MapFrom(src => src.UserId))
            .ForCtorParam(nameof(OrderResponseDto.OrderNumber),
                opt => opt.MapFrom(src => src.OrderNumber.Value))
            .ForCtorParam(nameof(OrderResponseDto.TotalPrice),
                opt => opt.MapFrom(src => src.TotalPrice.Amount))
            .ForCtorParam(nameof(OrderResponseDto.Currency),
                opt => opt.MapFrom(src => src.TotalPrice.Currency))
            .ForCtorParam(nameof(OrderResponseDto.Status),
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForCtorParam(nameof(OrderResponseDto.CreatedAt),
                opt => opt.MapFrom(src => src.CreatedAt))
            .ForCtorParam(nameof(OrderResponseDto.Items),
                opt => opt.MapFrom(src => src.Items));
        
        CreateMap<OrderItem, OrderItemResponseDto>()
            .ForMember(
                dest => dest.ProductName,
                opt => opt.MapFrom(src => src.ProductName)
            )
            .ForMember(
                dest => dest.UnitPrice,
                opt => opt.MapFrom(src => src.Price.Amount)
            )
            .ForMember(
                dest => dest.Currency,
                opt => opt.MapFrom(src => src.Price.Currency)
            );
        #endregion
    }
}