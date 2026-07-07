namespace ShoppingSystem.Microservice.Application.Common;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(
        this IQueryable<T> query,
        QueryCriteria criteria)
    {
        return query
            .Skip(criteria.Skip)
            .Take(criteria.PageSize);
    }
}