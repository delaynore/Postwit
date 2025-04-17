using Microsoft.Extensions.DependencyInjection;

namespace Postwit.DateTimeProvider;

public static class DependencyInjection
{
    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}

