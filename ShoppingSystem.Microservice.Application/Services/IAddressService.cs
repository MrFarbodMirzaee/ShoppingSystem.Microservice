using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace ShoppingSystem.Microservice.Application.Services;

/// <summary>
/// ToDo:add use cases interfaces
/// </summary>
public interface IAddressService
: IRepository<Address>
{
    #region Gets
    Task<Response<PagedResult<Address>>> GetByCityAsync(
        string city,
        QueryCriteria queryCriteria,
        CancellationToken ct);

    Task<Response<PagedResult<Address>>> GetByStateAsync(
        QueryCriteria queryCriteria,
        string state,
        CancellationToken ct);

    Task<Response<PagedResult<Address>>> GetByCountryAsync(
        QueryCriteria queryCriteria,
        string country,
        CancellationToken ct);

    Task<Response<PagedResult<Address>>> GetByPostalCodeAsync(
        QueryCriteria queryCriteria,
        string postalCode,
        CancellationToken ct);
        #endregion

    Task<Response<bool>> ExistsAsync(
        string street,
        string city,
        string state,
        string country,
        string postalCode,
        CancellationToken ct);
}