namespace DogsHouse.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    /// <summary>
    /// Repository for dogs.
    /// </summary>
    IDogsRepository DogsRepository { get; }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
