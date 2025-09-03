using Microsoft.EntityFrameworkCore;
using Postwit.Application;
using Postwit.Domain;
using Postwit.Infrastructure.Configurations;

namespace Postwit.Infrastructure;

internal sealed class ApplicationDbContext : DbContext, IWriteDbContext, IReadDbContext
{
    public DbSet<Tag> Tags { get; set; }

    public DbSet<Article> Articles { get; set; }
    
    public IQueryable<Article> ReadArticles => Articles.AsNoTracking().AsQueryable();

    public IQueryable<Tag> ReadTags => Tags.AsNoTracking().AsQueryable();

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
