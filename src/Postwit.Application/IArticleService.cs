using ErrorOr;
using Postwit.Domain;

namespace Postwit.Application;

public interface IArticleService
{
    Task<ErrorOr<Article>> GetArticleById(Guid articleId, CancellationToken cancellationToken);

    Task<ErrorOr<List<Article>>> GetArticles(CancellationToken cancellationToken);

    Task<ErrorOr<Created>> CreateArticle(CancellationToken cancellationToken);

    Task<ErrorOr<Updated>> UpdateArticle(CancellationToken cancellationToken);

    Task<ErrorOr<Deleted>> DeleteArticle(CancellationToken cancellationToken);
}
