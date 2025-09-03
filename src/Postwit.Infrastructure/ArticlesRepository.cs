using Microsoft.EntityFrameworkCore;
using Postwit.Application;
using Postwit.Application.Articles;
using Postwit.Domain;

namespace Postwit.Infrastructure;

internal sealed class ArticlesRepository(IWriteDbContext dbContext) : IArticlesRepository
{
    public async Task AddAsync(Article article, CancellationToken cancellationToken)
    {
        await dbContext.Articles.AddAsync(article, cancellationToken);
    }

    public async Task<Article?> GetByIdAsync(Guid articleId, CancellationToken cancellationToken)
    {
        return await dbContext.Articles.FirstOrDefaultAsync(x => x.Id == articleId, cancellationToken);
    }

    public async Task DeleteAsync(Guid articleId, CancellationToken cancellationToken)
    {
        await dbContext.Articles
            .Where(x => x.Id == articleId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
