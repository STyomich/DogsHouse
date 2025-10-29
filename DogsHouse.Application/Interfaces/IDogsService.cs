using DogsHouse.Application.DTOs;

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
}
