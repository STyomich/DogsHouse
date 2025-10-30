using DogsHouse.Application.Filters;
using DogsHouse.Application.Interfaces;

namespace DogsHouse.Application.Pipelines;

/// <summary>
/// A pipeline to apply multiple query filters to an IQueryable collection.
/// </summary>
/// <typeparam name="T">The type of the entities in the IQueryable collection.</typeparam>
public class QueryFilterPipeline<T>
{
    /// <summary>
    /// The list of query filters in the pipeline.
    /// </summary>
    private readonly List<IQueryFilter<T>> _filters = new();

    /// <summary>
    /// Adds a query filter to the pipeline.
    /// </summary>
    /// <param name="filter">The query filter to add.</param>
    /// <returns>The updated query filter pipeline.</returns>
    public QueryFilterPipeline<T> Add(IQueryFilter<T> filter)
    {
        _filters.Add(filter);
        return this;
    }

    /// <summary>
    /// Applies all added filters to the given query using the specified filter parameters.
    /// </summary>
    /// <param name="query">The IQueryable collection to filter.</param>
    /// <param name="filters">The filter parameters to apply.</param>
    /// <returns>The filtered IQueryable collection.</returns>
    public IQueryable<T> ApplyFilters(IQueryable<T> query, FilterParams filterParams)
    {
        foreach (var filter in _filters)
        {
            query = filter.Apply(query, filterParams);
        }
        return query;
    }
}
