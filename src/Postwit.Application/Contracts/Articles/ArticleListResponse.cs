namespace Postwit.Application.Contracts.Articles;

public sealed record ArticleListResponse(IEnumerable<ArticleDto> Items);
