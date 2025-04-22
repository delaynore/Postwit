using Microsoft.EntityFrameworkCore;
using Postwit.Domain;

namespace Postwit.Application;

public interface IArticleRepository
{
    DbSet<Article> Articles { get; }
}
