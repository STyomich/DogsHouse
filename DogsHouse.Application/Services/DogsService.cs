using DogsHouse.Application.DTOs;
using DogsHouse.Application.Exceptions;
using DogsHouse.Application.Interfaces;
using DogsHouse.Application.Interfaces.Repositories;
using DogsHouse.Application.Models;

namespace DogsHouse.Application.Services;

public class DogsService(IUnitOfWork unitOfWork) : IDogsService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task AddDogAsync(AddDogRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.DogsRepository.AddAsync(MapAddDogRequestToDog(request), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<DogDto>> GetAllDogsAsync(CancellationToken cancellationToken)
    {
        var dogs = await _unitOfWork.DogsRepository.GetAllDogsAsync(cancellationToken);
        return dogs.Select(dog => new DogDto
        {
            Name = dog.Name,
            Color = dog.Color,
            TailLength = dog.TailLength,
            Weight = dog.Weight
        });
    }

    public async Task<DogDto> GetDogByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var dog = await _unitOfWork.DogsRepository.GetDogByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Dog not found");

        return new DogDto
        {
            Name = dog.Name,
            Color = dog.Color,
            TailLength = dog.TailLength,
            Weight = dog.Weight
        };
    }

    /// <summary>
    /// Maps AddDogRequest to Dog model
    /// </summary>
    /// <param name="request">AddDogRequest entity.</param>
    /// <returns>Dog entity.</returns>
    private static Dog MapAddDogRequestToDog(AddDogRequest request) => new Dog
    {
        Name = request.Name,
        Color = request.Color,
        TailLength = request.TailLength,
        Weight = request.Weight
    };
}
