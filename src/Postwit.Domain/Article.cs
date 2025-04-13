namespace Postwit.Domain;

public sealed class Article
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public string Title { get; set; }

    public string Content { get; set; }

    public Guid AuthorId { get; set; }

    public TimeSpan ReadTime { get; set; }

    public string ImageUrl { get; set; }

    public string Slug { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
