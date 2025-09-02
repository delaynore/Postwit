using Microsoft.EntityFrameworkCore;
using Postwit.Application.Abstactions;
using Postwit.Application.Contracts.Articles;
using Postwit.Application.Mappers;

namespace Postwit.Application.Articles.Queries.GetArticles;

internal sealed class GetArticlesQueryHandler : IQueryHandler<GetArticlesQuery, ArticleListResponse>
{
    private readonly IArticleRepository _repository;

    public GetArticlesQueryHandler(IArticleRepository repository)
    {
        _repository = repository;
    }

    public async Task<ArticleListResponse> Handle(GetArticlesQuery query, CancellationToken cancellationToken)
    {
        var records = await _repository.Articles
            .Select(ArticlesProjections.ToDto())
            .ToListAsync(cancellationToken);

        return new ArticleListResponse(records);
    }
}
