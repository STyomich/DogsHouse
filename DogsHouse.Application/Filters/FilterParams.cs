namespace DogsHouse.Application.Filters;

public class FilterParams
{
    public string? Attribute { get; set; } // Weight, Color, Name...

    public string? Order { get; set; } // ASC, DESC

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
