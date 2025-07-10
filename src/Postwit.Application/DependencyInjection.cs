using Microsoft.Extensions.DependencyInjection;
using Postwit.Application.Abstactions;
using Postwit.Application.Articles.Commands.PublishArticle;

namespace Postwit.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITagService, TagService>();

        services.AddScoped<ICommandHandler<PublishArticleCommand, Guid>, PublishArticleHandler>();

        return services;
    }
}
