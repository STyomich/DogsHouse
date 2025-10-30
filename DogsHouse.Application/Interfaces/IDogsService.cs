using DogsHouse.Application.DTOs;
using DogsHouse.Application.Filters;

namespace DogsHouse.Application.Interfaces;

public interface IDogsService
{
    /// <summary>
    /// Adds a new dog to the system
    /// </summary>
    /// <param name="request">AddDogRequest entity.</param>
    Task AddDogAsync(AddDogRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all dogs from the system
    /// </summary>
    /// <returns>List of DogDto entities.</returns>
    Task<IEnumerable<DogDto>> GetAllDogsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a dog by filter params.
    /// </summary>
    /// <param name="filterParams">Filter parameters.</param>
    /// <returns>SortedDogsResponse which contains the sorted list of dogs and pagination information.</returns>
    Task<SortedDogsResponse> GetSortedDogsAsync(FilterParams filterParams, CancellationToken cancellationToken);
}
