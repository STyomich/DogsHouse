namespace DogsHouse.Application.Models;

public class Dog
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;

    public int TailLength { get; set; }

    public int Weigth { get; set; }
}
