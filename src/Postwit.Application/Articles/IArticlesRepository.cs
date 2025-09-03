using Postwit.Domain;

namespace Postwit.Application.Articles;

public interface IArticlesRepository
{
    Task AddAsync(Article article, CancellationToken cancellationToken);
    
    Task<Article?> GetByIdAsync(Guid articleId, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid articleId, CancellationToken cancellationToken);
    
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
