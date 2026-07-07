using AutoMapper;
using ShoppingSystem.Microservice.Application.Dtos;

namespace ShoppingSystem.Microservice.Application.Profiles.QueryCriteria;

public class QueryCriteriaProfile :  Profile
{
    public QueryCriteriaProfile()
    {
        #region PageEnation
        CreateMap<QueryCriteriaRequestDto, Common.QueryCriteria>()
            .ForMember(dest => dest.PageNumber,
                opt => opt.MapFrom(src => src.PageNumber))

            .ForMember(dest => dest.PageSize,
                opt => opt.MapFrom(src => src.PageSize));
        #endregion
    }
}