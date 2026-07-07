using Infrastructure.Contexts;
using Infrastructure.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Application.Attributes;
using ShoppingSystem.Microservice.Application.Common;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Aggregates.Product;
using ShoppingSystem.Microservice.Domain.Wrappers;

namespace Infrastructure.Persistence.Repositories;

[ScopedService]
public class CategoryRepository  
: Repository<Category>, ICategoryService 
{
    public CategoryRepository
    (AppDbContext context) : base(context)
    {
    }

    public async Task<Response<Category>> GetByNameAsync
        (string name, CancellationToken ct)
    {
        var category = await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Name == name,
                ct);

        return new Response
        <Category>(category);
    }

    public async Task<Response<bool>> ExistsByNameAsync
        (string name, CancellationToken ct)
    {
        var exists = await DbSet
            .AsNoTracking()
            .AnyAsync(
                x => x.Name == name,
                ct);

        return new Response
        <bool>(exists);;
    }

    public async Task<Response<PagedResult<Category>>> GetActiveCategoriesAsync(
        QueryCriteria criteria,
        CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Id);

        var totalCount = await query.CountAsync(ct);

        var categories = await query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize)
            .ToListAsync(ct);

        var pagedResult = new PagedResult<Category>
        {
            Items = categories,
            TotalCount = totalCount,
            PageNumber = criteria.PageNumber,
            PageSize = criteria.PageSize
        };

        return new Response
        <PagedResult<Category>>(pagedResult);
    }
}