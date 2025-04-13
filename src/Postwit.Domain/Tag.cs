namespace Postwit.Domain;

public sealed class Tag
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public required string Name { get; set; }

    public string? Description { get; set; }
}
