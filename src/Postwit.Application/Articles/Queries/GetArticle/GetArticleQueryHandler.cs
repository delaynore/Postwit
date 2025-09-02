using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Postwit.Application.Abstactions;
using Postwit.Application.Contracts.Articles;
using Postwit.Application.Mappers;

namespace Postwit.Application.Articles.Queries.GetArticle;

internal sealed class GetArticleQueryHandler : IQueryHandler<GetArticleQuery, ErrorOr<ArticleDto>>
{
    private readonly IArticleRepository _repository;
    
    public GetArticleQueryHandler(IArticleRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ErrorOr<ArticleDto>> Handle(GetArticleQuery query, CancellationToken cancellationToken)
    {
        var dbQuery = _repository.Articles.AsQueryable();

        dbQuery = Guid.TryParse(query.IdOrSlug, out var id) 
            ? dbQuery.Where(a => a.Id == id) 
            : dbQuery.Where(a => a.Slug == query.IdOrSlug);

        var record = await dbQuery.Select(ArticlesProjections.ToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (record is null)
        {
            return Error.NotFound("article.not_found",$"No article found with id: {query.IdOrSlug}");
        }
        
        return record;
    }
}
