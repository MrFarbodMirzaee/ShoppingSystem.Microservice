using AutoMapper;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Domain.Entities;

namespace ShoppingSystem.Microservice.Application.Profiles.Cart;

public class CartProfile : Profile
{
    public CartProfile()
    {
        #region Commands

        

        #endregion
        #region Queries
        CreateMap<Domain.Aggregates.Cart.Cart, CartResponseDto>()
            .ForCtorParam(nameof(CartResponseDto.Id),
                opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(CartResponseDto.CustomerId),
                opt => opt.MapFrom(src => src.CustomerId))
            .ForCtorParam(nameof(CartResponseDto.CartItems),
                opt => opt.MapFrom(src => src.Items))
            .ForCtorParam(nameof(CartResponseDto.TotalPrice),
                opt => opt.MapFrom(src => src.GetTotalPrice()));
        
        CreateMap<CartItem, CartItemResponseDto>()
            .ForCtorParam(nameof(CartItemResponseDto.Id),
                opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(CartItemResponseDto.ProductId),
                opt => opt.MapFrom(src => src.ProductId))
            .ForCtorParam(nameof(CartItemResponseDto.ProductName),
                opt => opt.MapFrom(src => src.ProductName.Value))
            .ForCtorParam(nameof(CartItemResponseDto.Quantity),
                opt => opt.MapFrom(src => src.Quantity.Value))
            .ForCtorParam(nameof(CartItemResponseDto.UnitPrice),
                opt => opt.MapFrom(src => src.UnitPrice))
            .ForCtorParam(nameof(CartItemResponseDto.TotalPrice),
                opt => opt.MapFrom(src => src.GetTotalPrice()));
        #endregion
    }
}