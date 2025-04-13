using Microsoft.Extensions.DependencyInjection;

namespace Postwit.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITagService, TagService>();

        return services;
    }
}
