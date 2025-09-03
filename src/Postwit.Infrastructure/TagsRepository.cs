using Microsoft.EntityFrameworkCore;
using Postwit.Application;
using Postwit.Application.Tags;
using Postwit.Domain;

namespace Postwit.Infrastructure;

public class TagsRepository(IWriteDbContext dbContext) : ITagsRepository
{
    public async Task AddAsync(Tag tag, CancellationToken cancellationToken)
    {
        await dbContext.Tags.AddAsync(tag, cancellationToken);
    }

    public async Task<Tag?> GetByIdAsync(Guid tagId, CancellationToken cancellationToken)
    {
        return await dbContext.Tags.FirstOrDefaultAsync(x => x.Id == tagId, cancellationToken);
    }
    
    public async Task<Tag?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await dbContext.Tags.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task DeleteAsync(Guid tagId, CancellationToken cancellationToken)
    {
        await dbContext.Tags
            .Where(x => x.Id == tagId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
