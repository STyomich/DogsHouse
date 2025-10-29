using DogsHouse.Application.DTOs;
using DogsHouse.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DogsHouse.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DogsController(IDogsService dogsService) : ControllerBase
{
    private readonly IDogsService dogsService = dogsService;

    [HttpPost("dogs")]
    public async Task<IActionResult> AddDog(AddDogRequest request, CancellationToken cancellationToken)
    {
        await dogsService.AddDogAsync(request, cancellationToken);
        return NoContent();
    }
}
