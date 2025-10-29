namespace DogsHouse.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IDogsRepository DogsRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
