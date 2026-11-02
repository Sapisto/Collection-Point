namespace CollectPoint.Domain.QueryParameters;

public class CustomerQueryParameters
{

    public string? Query { get; set; }
    public string? Currency { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
