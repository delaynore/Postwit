using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Postwit.Application;

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

        services.AddScoped<IUnitOfWork>(c => c.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ITagRepository>(c => c.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
