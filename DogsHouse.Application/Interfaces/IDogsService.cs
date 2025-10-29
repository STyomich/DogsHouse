using DogsHouse.Application.DTOs;

namespace DogsHouse.Application.Interfaces;

public interface IDogsService
{
    Task AddDogAsync(AddDogRequest request, CancellationToken cancellationToken);
}
