using ErrorOr;
using Postwit.Domain;

namespace Postwit.Application;

internal sealed class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _uof;

    public ArticleService(IUnitOfWork uof)
    {
        _uof = uof;
    }

    public Task<ErrorOr<Created>> CreateArticle(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Deleted>> DeleteArticle(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Article>> GetArticleById(Guid articleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<List<Article>>> GetArticles(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Updated>> UpdateArticle(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
