using AutoMapper;
using ShoppingSystem.Microservice.Application.Dtos;

namespace ShoppingSystem.Microservice.Application.Profiles.Product;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        #region Queries
        CreateMap<Domain.Aggregates.Product.Product, ProductResponseDto>()
            .ForCtorParam(nameof(ProductResponseDto.Id),
                opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(ProductResponseDto.Name),
                opt => opt.MapFrom(src => src.Name.Value))
            .ForCtorParam(nameof(ProductResponseDto.Description),
                opt => opt.MapFrom(src => src.Description))
            .ForCtorParam(nameof(ProductResponseDto.Price),
                opt => opt.MapFrom(src => src.Price.Amount))
            .ForCtorParam(nameof(ProductResponseDto.Currency),
                opt => opt.MapFrom(src => src.Price.Currency))
            .ForCtorParam(nameof(ProductResponseDto.CategoryId),
                opt => opt.MapFrom(src => src.CategoryId))
            .ForCtorParam(nameof(ProductResponseDto.IsAvailable),
                opt => opt.MapFrom(src => src.IsAvailable))
            .ForCtorParam(nameof(ProductResponseDto.Images),
                opt => opt.MapFrom(src => src.Images));

        #endregion
    }
}