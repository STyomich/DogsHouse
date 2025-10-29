using DogsHouse.Application.DTOs;
using DogsHouse.Application.Interfaces;

namespace DogsHouse.Application.Services;

public class DogsService : IDogsService
{
    public Task AddDogAsync(AddDogRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
