namespace DogsHouse.Application.DTOs;

public class SortedDogsResponse
{
    public IEnumerable<DogDto>? Dogs { get; set; }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }
}
