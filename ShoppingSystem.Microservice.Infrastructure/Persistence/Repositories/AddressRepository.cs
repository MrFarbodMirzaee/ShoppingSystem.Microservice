using Infrastructure.Contexts;
using Infrastructure.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Application.Attributes;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace Infrastructure.Persistence.Repositories;

[ScopedService]
public class AddressRepository
: Repository<Address>, IAddressService
{
    public AddressRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Response<PagedResult<Address>>> GetByCityAsync(
        string city,
        QueryCriteria criteria,
        CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.City == city)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var result = new PagedResult<Address>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
        <PagedResult<Address>>(result);
    }

    public async Task<Response<PagedResult<Address>>> GetByStateAsync(
        QueryCriteria criteria,
        string state,
        CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.State == state)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var addresses = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Address>
        {
            Items = addresses,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
        <PagedResult<Address>>(pagedResult);
    }

    public async Task<Response<PagedResult<Address>>> GetByCountryAsync
    (QueryCriteria criteria, string country, CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.Country == country)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var addresses = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Address>
        {
            Items = addresses,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
        <PagedResult<Address>>(pagedResult);
    }

    public async Task<Response<PagedResult<Address>>> GetByPostalCodeAsync
    (QueryCriteria criteria, string postalCode, CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.PostalCode == postalCode)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var addresses = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Address>
        {
            Items = addresses,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
        <PagedResult<Address>>(pagedResult);
    }

    public async Task<Response<bool>> ExistsAsync(
        string street,
        string city,
        string state,
        string country,
        string postalCode,
        CancellationToken ct)
    {
        var exists = await DbSet
            .Where(x=> x.Street == street &&
                   x.City == city &&
                   x.State == state &&
                   x.Country == country &&
                   x.PostalCode == postalCode)
            .AnyAsync(ct);

        return new Response
        <bool>(exists);
    }
}