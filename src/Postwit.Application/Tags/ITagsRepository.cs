using Postwit.Domain;

namespace Postwit.Application.Tags;

public interface ITagsRepository
{
    Task AddAsync(Tag tag, CancellationToken cancellationToken);
    
    Task<Tag?> GetByIdAsync(Guid tagId, CancellationToken cancellationToken);
    
    Task<Tag?> GetByNameAsync(string name, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid tagId, CancellationToken cancellationToken);
    
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
