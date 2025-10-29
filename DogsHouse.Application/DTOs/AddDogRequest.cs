
using DogsHouse.Application.Validation.Attributes;

namespace DogsHouse.Application.DTOs;

public class AddDogRequest
{
    [UniqueDogName]
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;

    [TailLengthPositiveValue]
    public int TailLength { get; set; }

    public int Weight { get; set; }
}
