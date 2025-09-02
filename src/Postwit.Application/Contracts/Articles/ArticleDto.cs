using Postwit.Application.Contracts.Tags;

namespace Postwit.Application.Contracts.Articles;

public sealed record ArticleDto(
    Guid Id,
    string Title,
    string Content,
    Guid AuthorId,
    TimeSpan ReadTime,
    DateTime CreatedAtUtc,
    IEnumerable<Guid> TagsIds);
