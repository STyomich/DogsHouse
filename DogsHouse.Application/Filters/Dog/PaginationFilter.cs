using DogsHouse.Application.Interfaces;

namespace DogsHouse.Application.Filters.Dog;

public class PaginationFilter : IQueryFilter<Models.Dog>
{
    public IQueryable<Models.Dog> Apply(IQueryable<Models.Dog> query, FilterParams filterParams)
    {
        var pageNumber = filterParams.PageNumber < 1 ? 1 : filterParams.PageNumber;
        var pageSize = filterParams.PageSize < 1 ? 10 : filterParams.PageSize;

        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
