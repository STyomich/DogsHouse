using DogsHouse.Application.Interfaces;

namespace DogsHouse.Application.Filters.Dog;

public class SortingFilter : IQueryFilter<Models.Dog>
{
    public IQueryable<Models.Dog> Apply(IQueryable<Models.Dog> query, FilterParams filterParams)
    {
        if (!string.IsNullOrWhiteSpace(filterParams.Attribute))
        {
            var sort = filterParams.Attribute.Trim().ToLower(System.Globalization.CultureInfo.CurrentCulture);
            return (sort, filterParams.Order) switch
            {
                ("weight", "asc") => query.OrderBy(g => g.Weight),
                ("weight", "desc") => query.OrderByDescending(g => g.Weight),
                ("color", "asc") => query.OrderBy(g => g.Color),
                ("color", "desc") => query.OrderByDescending(g => g.Color),
                ("name", "asc") => query.OrderBy(g => g.Name),
                ("name", "desc") => query.OrderByDescending(g => g.Name),
                ("taillength", "asc") => query.OrderBy(g => g.TailLength),
                ("taillength", "desc") => query.OrderByDescending(g => g.TailLength),
                _ => query.OrderBy(g => g.Name),
            };
        }

        return query.OrderBy(g => g.Name);
    }
}
