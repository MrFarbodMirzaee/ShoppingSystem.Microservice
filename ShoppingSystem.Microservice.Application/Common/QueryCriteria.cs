namespace ShoppingSystem.Microservice.Application.Common;

public class QueryCriteria
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int Skip => (PageNumber - 1) * PageSize;
}