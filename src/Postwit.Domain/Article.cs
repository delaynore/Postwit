using System.Text;
using ErrorOr;
using Postwit.DateTimeProvider;

namespace Postwit.Domain;

public sealed class Article
{
    public const int MaxTitleLength = 256;
    public const int MinTitleLength = 2;

    public const int MaxContentLength = 1024 * 128;

    public Guid Id { get; init; } = Guid.CreateVersion7();

    public required string Title { get; set; }

    public required string Content { get; set; }

    public Guid AuthorId { get; init; }

    public TimeSpan ReadTime { get; private set; }

    public string? PreviewImageUrl { get; set; }

    public string Slug { get; private set; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; set; }

    public ArticleStatus Status { get; set; }

    private readonly List<Guid> _tagsIds = [];

    public IReadOnlyCollection<Guid> TagsIds => _tagsIds;

    private readonly List<Guid> _filesIds = [];

    public IReadOnlyCollection<Guid> FilesIds => _filesIds;

    private Article()
    { }

    public static ErrorOr<Article> Create(
        Guid articleId,
        Guid authorId, 
        string title,
        string content, 
        string? previewImageUrl,
        ArticleStatus status,
        IDateTimeProvider dateTimeProvider)
    {
        content = content?.Trim() ?? string.Empty;
        var errors = Validate(title, content, status);

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Article()
        {
            Id = articleId,
            Title = title,
            AuthorId = authorId,
            Content = content,
            PreviewImageUrl = previewImageUrl,
            Status = status,
            CreatedAtUtc = dateTimeProvider.UtcNow,
            ReadTime = CalculateReadTime(content),
            Slug = GenerateSlug(title)
        };
    }

    private static TimeSpan CalculateReadTime(string content)
    {
        const int AverageReadCharactersPerMinute = 500;
        return TimeSpan.FromMinutes((double) content.Length / AverageReadCharactersPerMinute);
    }

    private static List<Error> Validate(string title, string content, ArticleStatus articleStatus)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(title))
        {
            errors.Add(Error.Validation("Article.Title", "Must be non empty string"));
        }

        if (articleStatus is not ArticleStatus.Drafted and not ArticleStatus.Published)
        {
            errors.Add(Error.Validation("Article.Status", "Article status must be \"Drafted\" or \"Published\" "));
        }

        if (content.Length > MaxContentLength)
        {
            errors.Add(Error.Validation("Article.Content", $"Article content must be shorter than {MaxContentLength}"));
        }

        return errors;
    }

    private static readonly char[] ReplaceableChars = @" `~!@#$%^&*()+=_\|]}[{;:'""/.>,<?".ToCharArray();

    private static string GenerateSlug(string title)
    {
        var titleSplit = title.AsSpan().SplitAny(ReplaceableChars);
        var slug = new StringBuilder(title.Length);
        foreach (var range in titleSplit)
        {
            if (string.IsNullOrWhiteSpace(title[range]))
            {
                continue;
            }
            slug.Append(title[range]);
            slug.Append('-');
        }
        slug.Remove(slug.Length - 1, 1);
        return slug.ToString();
    }
}

public enum ArticleStatus
{
    None = 0,
    Drafted = 1,
    Published = 2,
}
