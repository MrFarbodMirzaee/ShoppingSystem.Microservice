using AutoMapper;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Dtos;
using ShoppingSystem.Microservice.Application.UseCases.Address.Commands.Add;
using ShoppingSystem.Microservice.Application.UseCases.Address.Commands.Update;

namespace ShoppingSystem.Microservice.Application.Profiles.Address;

public class AddressProfile 
: Profile
{
    public AddressProfile()
    {
        #region Commands
        CreateMap<AddAddressRequestCommand, Domain.Entities.Address>()
            .ConstructUsing(src => new Domain.Entities.Address(
                src.Street,
                src.City,
                src.State,
                src.Country,
                src.PostalCode));
        
        CreateMap<UpdateAddressCommandRequest, Domain.Entities.Address>()
            .ConstructUsing(src => new Domain.Entities.Address(
                src.Street,
                src.City,
                src.State,
                src.Country,
                src.PostalCode));
        
        #endregion

        #region Queries
        CreateMap<Domain.Entities.Address, AddressResponseDto>();
        CreateMap<PagedResult<Domain.Entities.Address>, List<AddressResponseDto>>();

        #endregion
    }
}