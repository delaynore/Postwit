using Postwit.Domain;

namespace Postwit.Application;

public interface IReadDbContext
{
    IQueryable<Tag> ReadTags { get; }
    
    IQueryable<Article> ReadArticles { get; }
}
