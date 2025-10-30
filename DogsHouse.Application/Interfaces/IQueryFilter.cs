using DogsHouse.Application.Filters;

namespace DogsHouse.Application.Interfaces;

/// <summary>
/// Interface for applying query filters to an IQueryable collection.
/// </summary>
/// <typeparam name="T">The type of the entities in the IQueryable collection.</typeparam>
public interface IQueryFilter<T>
{
    /// <summary>
    /// Applies the specified filter parameters to the given query.
    /// </summary>
    /// <param name="query">The IQueryable collection to filter.</param>
    /// <param name="filterParams">The filter parameters to apply.</param>
    IQueryable<T> Apply(IQueryable<T> query, FilterParams filterParams);
}
