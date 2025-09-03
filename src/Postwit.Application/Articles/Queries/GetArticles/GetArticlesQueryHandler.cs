using Microsoft.EntityFrameworkCore;
using Postwit.Application.Abstactions;
using Postwit.Application.Contracts.Articles;
using Postwit.Application.Mappers;

namespace Postwit.Application.Articles.Queries.GetArticles;

internal sealed class GetArticlesQueryHandler : IQueryHandler<GetArticlesQuery, ArticleListResponse>
{
    private readonly IReadDbContext _readDbContext;

    public GetArticlesQueryHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<ArticleListResponse> Handle(GetArticlesQuery query, CancellationToken cancellationToken)
    {
        var records = await _readDbContext.ReadArticles
            .Select(ArticlesProjections.ToDto())
            .ToListAsync(cancellationToken);

        return new ArticleListResponse(records);
    }
}
