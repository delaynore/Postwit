using Microsoft.Extensions.DependencyInjection;
using Postwit.Application.Abstactions;
using Postwit.Application.Articles.Commands.DeleteArticle;
using Postwit.Application.Articles.Commands.PublishArticle;
using Postwit.Application.Articles.Commands.PublishDraftedArticle;
using Postwit.Application.Articles.Commands.UpdateArticle;
using Postwit.Application.Articles.Queries.GetArticle;
using Postwit.Application.Articles.Queries.GetArticles;
using Postwit.Application.Contracts.Articles;

namespace Postwit.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITagService, TagService>();

        services.AddScoped<ICommandHandler<PublishArticleCommand, Guid>, PublishArticleHandler>();
        services.AddScoped<ICommandHandler<PublishDraftedArticleCommand>, PublishDraftedArticleCommandHandler>();
        
        services.AddScoped<ICommandHandler<DeleteArticleCommand>, DeleteArticleHandler>();
        services.AddScoped<ICommandHandler<UpdateArticleCommand>, UpdateArticleCommandHandler>();
        
        services.AddScoped<IQueryHandler<GetArticlesQuery, ArticleListResponse>, GetArticlesQueryHandler>();
        services.AddScoped<IQueryHandler<GetArticleQuery, ErrorOr.ErrorOr<ArticleDto>>, GetArticleQueryHandler>();
        
        return services;
    }
}
