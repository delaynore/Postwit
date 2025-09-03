using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Postwit.Application;
using Postwit.Application.Articles;
using Postwit.Application.Tags;

namespace Postwit.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") 
            ?? throw new ArgumentNullException(nameof(configuration), "The connection string was not provided.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IReadDbContext>(c => c.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IWriteDbContext>(c => c.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IArticlesRepository, ArticlesRepository>();
        services.AddScoped<ITagsRepository, TagsRepository>();
        
        return services;
    }
}
