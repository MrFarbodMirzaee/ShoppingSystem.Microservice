using AutoMapper;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.UseCases.Category.Commands.Add;

namespace ShoppingSystem.Microservice.Application.Profiles.Category;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        #region Queries

        CreateMap<Domain.Aggregates.Product.Category, CategoryResponseDto>()
            .ForCtorParam(nameof(CategoryResponseDto.Id),
                opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(CategoryResponseDto.Name),
                opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(CategoryResponseDto.Description),
                opt => opt.MapFrom(src => src.Description))
            .ForCtorParam(nameof(CategoryResponseDto.ParentCategoryId),
                opt => opt.MapFrom(src => src.ParentCategoryId))
            .ForCtorParam(nameof(CategoryResponseDto.IsActive),
                opt => opt.MapFrom(src => src.IsActive));

        CreateMap<AddCategoryCommand, Domain.Aggregates.Product.Category>()
            .ConstructUsing(src => new Domain.Aggregates.Product.Category(
                src.Name,
                src.Description,
                src.ParentCategoryId
            ));

        #endregion
    }
}