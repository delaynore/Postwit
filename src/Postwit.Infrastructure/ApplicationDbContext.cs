using Microsoft.EntityFrameworkCore;
using Postwit.Application;
using Postwit.Domain;
using Postwit.Infrastructure.Configurations;

namespace Postwit.Infrastructure;

internal sealed class ApplicationDbContext : DbContext, IUnitOfWork, ITagRepository
{
    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<Article> Articles => Set<Article>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
    }
}
