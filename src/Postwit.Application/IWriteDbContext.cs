using Microsoft.EntityFrameworkCore;
using Postwit.Domain;

namespace Postwit.Application;

public interface IWriteDbContext
{
    DbSet<Article> Articles { get; }
    
    DbSet<Tag> Tags { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
