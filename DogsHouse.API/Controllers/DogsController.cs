using DogsHouse.Application.DTOs;
using DogsHouse.Application.Filters;
using DogsHouse.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DogsHouse.API.Controllers;

[ApiController]
[Route("dogs")]
public class DogsController(IDogsService dogsService) : ControllerBase
{
    private readonly IDogsService _dogsService = dogsService;

    /// <summary>
    /// Adds a new dog to the system.
    /// </summary>
    /// <param name="request">The request containing the dog details.</param>
    /// <returns>A response indicating the result of the add operation.</returns>
    [HttpPost]
    public async Task<IActionResult> AddDog(AddDogRequest request, CancellationToken cancellationToken)
    {
        await _dogsService.AddDogAsync(request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Retrieves a dog by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the dog.</param>
    /// <returns>The dog details if found; otherwise, a not found response.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDogById(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a sorted and filtered list of dogs.
    /// This endpoint is used to fetch a list of dogs based on various filtering and sorting criteria.
    /// </summary>
    /// <param name="filterParams">Parameters for filtering and sorting the dogs.</param>
    /// <returns>Response containing a sorted and filtered list of dogs along with pagination details.</returns>
    [HttpGet]
    public async Task<IActionResult> GetSortedDogs([FromQuery] FilterParams filterParams, CancellationToken cancellationToken)
    {
        var result = await _dogsService.GetSortedDogsAsync(filterParams, cancellationToken);
        return Ok(result);
    }
}
