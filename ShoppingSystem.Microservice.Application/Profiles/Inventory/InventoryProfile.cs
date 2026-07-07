using AutoMapper;
using ShoppingSystem.Microservice.Application.Dtos;

namespace ShoppingSystem.Microservice.Application.Profiles.Inventory;

public class InventoryProfile : Profile
{
    public InventoryProfile()
    {
        #region Queries
        CreateMap<Domain.Aggregates.Inventory.Inventory, InventoryResponseDto>()
            .ForMember(
                dest => dest.Quantity,
                opt => opt.MapFrom(src => src.Quantity.Value)
            )
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString())
            );

        #endregion
    }
}