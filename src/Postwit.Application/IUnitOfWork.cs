using Microsoft.EntityFrameworkCore;
using Postwit.Domain;

namespace Postwit.Application;

public interface IUnitOfWork
{
    DbSet<Tag> Tags { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public interface ITagRepository
{
    DbSet<Tag> Tags { get; }
}
