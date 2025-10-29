using DogsHouse.Application.Models;

namespace DogsHouse.Application.Interfaces.Repositories;

public interface IDogsRepository
{
    /// <summary>
    /// Gets all dogs which existed in database.
    /// </summary>
    /// <returns>Collection of dogs.</returns>
    Task<ICollection<Dog>> GetAllDogsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets a dog by identifier.
    /// </summary>
    /// <param name="id">Identifier of dog.</param>
    /// <returns>Dog.</returns>
    Task<Dog> GetDogByIdAsync(Guid id,  CancellationToken cancellationToken);
    
    /// <summary>
    /// Adds a dog entity into database.
    /// </summary>
    /// <param name="dog">Dog entity.</param>
    Task AddAsync(Dog dog, CancellationToken cancellationToken);

    /// <summary>
    /// Checks is exists a dog with supplied name.
    /// </summary>
    /// <param name="name">Name of dog</param>
    /// <returns>Boolean type</returns>
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Returns a query of dogs dbset.
    /// </summary>
    IQueryable Query();
}
