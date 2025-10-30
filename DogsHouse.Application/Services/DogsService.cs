using DogsHouse.Application.DTOs;
using DogsHouse.Application.Exceptions;
using DogsHouse.Application.Filters;
using DogsHouse.Application.Filters.Dog;
using DogsHouse.Application.Interfaces;
using DogsHouse.Application.Interfaces.Repositories;
using DogsHouse.Application.Models;
using DogsHouse.Application.Pipelines;
using Microsoft.EntityFrameworkCore;

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

    public async Task<SortedDogsResponse> GetSortedDogsAsync(FilterParams filterParams, CancellationToken cancellationToken)
    {
        // Phase 1: Get the filtered and paginated dogs
        var pipeline = BuildPipeline();
        var filteredQuery = ApplyFilters(pipeline, filterParams);
        var paginatedDogs = await ApplyPagination(filteredQuery, filterParams, cancellationToken);

        // Phase 2: Get the total count of pages and returning a response.
        var totalPages = GetTotalPages(filterParams, paginatedDogs.Count);
        return ToSortedDogsResponse(paginatedDogs, totalPages, filterParams);
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

    private static QueryFilterPipeline<Dog> BuildPipeline()
    {
        return new QueryFilterPipeline<Dog>()
            .Add(new SortingFilter());
    }

    private IQueryable<Dog> ApplyFilters(QueryFilterPipeline<Dog> pipeline, FilterParams filterParams)
    {
        return pipeline.ApplyFilters(_unitOfWork.DogsRepository.Query(), filterParams);
    }

    private static async Task<List<DogDto>> ApplyPagination(IQueryable<Dog> filteredQuery, FilterParams filterParams, CancellationToken cancellationToken)
    {
        var paginatedQuery = new PaginationFilter().Apply(filteredQuery, filterParams);

        return await paginatedQuery
            .Select(d => new DogDto
            {
                Name = d.Name,
                Color = d.Color,
                TailLength = d.TailLength,
                Weight = d.Weight
            })
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Recalculation page count for receiving products to avoid big amount of data.
    /// </summary>
    /// <param name="currentPageCount">Page count in filter params.</param>
    /// <param name="totalDogs">Total amount of dogs.</param>
    /// <returns>Page count as string.</returns>
    private static string UpdatePageCount(int currentPageCount, int totalDogs)
    {
        return (currentPageCount - totalDogs).ToString();
    }

    /// <summary>
    /// Get total pages in pagination by filter params and total dogs.
    /// </summary>
    /// <param name="filterParams">Filter params for setting up pipeline.</param>
    /// <param name="totalDogs">Total dogs which returned by filter params.</param>
    /// <returns>Total pages.</returns>
    private static int GetTotalPages(FilterParams filterParams, int totalDogs)
    {
        const int defaultPageCount = 10;
        int pageCount = defaultPageCount;

        pageCount = filterParams.PageSize > 0 ? filterParams.PageSize : defaultPageCount;

        // Calculate total pages
        int totalPages = (int)Math.Ceiling(totalDogs / (double)pageCount);
        return totalPages;
    }

    /// <summary>
    /// Returns SortedDogsResponse by dogs, total pages and filter params.
    /// </summary>
    /// <param name="dogs">Dogs.</param>
    /// <param name="totalPages">Total pages.</param>
    /// <param name="filterParams">Filter params.</param>
    /// <returns>SortedDogsResponse.</returns>
    private static SortedDogsResponse ToSortedDogsResponse(IEnumerable<DogDto> dogs, int totalPages, FilterParams filterParams)
    {
        return new SortedDogsResponse
        {
            Dogs = dogs.ToList(),
            TotalPages = totalPages,
            CurrentPage = filterParams.PageNumber,
        };
    }
}
