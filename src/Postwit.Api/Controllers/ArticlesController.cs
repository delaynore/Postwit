using Microsoft.AspNetCore.Mvc;
using Postwit.Application.Abstactions;
using Postwit.Application.Articles.Commands.DeleteArticle;
using Postwit.Application.Articles.Commands.PublishArticle;
using Postwit.Application.Articles.Commands.PublishDraftedArticle;
using Postwit.Application.Articles.Commands.UpdateArticle;
using Postwit.Application.Articles.Queries.GetArticle;
using Postwit.Application.Articles.Queries.GetArticles;
using Postwit.Application.Contracts.Articles;

namespace Postwit.Api.Controllers;

[ApiController]
[Route("api/articles")]
public sealed class ArticlesController : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetArticle(
        [FromServices] IQueryHandler<GetArticlesQuery, ArticleListResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetArticlesQuery(), cancellationToken);

        return Results.Ok(result);
    }
    
    [HttpGet("{idOrSlug}")]
    public async Task<IResult> GetArticle([FromRoute] string idOrSlug,
        [FromServices] IQueryHandler<GetArticleQuery, ErrorOr.ErrorOr<ArticleDto>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetArticleQuery(idOrSlug), cancellationToken);

        return result.Match(Results.Ok, Results.NotFound);
    }
    
    [HttpPost]
    public async Task<IResult> PublishArticle(
        [FromBody] CreateArticleDto dto,
        [FromServices] ICommandHandler<PublishArticleCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new PublishArticleCommand(dto), cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }

    [HttpPut("{articleId}/publish")]
    public async Task<IResult> PublishDraftedArticle(
        [FromRoute] Guid articleId,
        [FromServices] ICommandHandler<PublishDraftedArticleCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new PublishDraftedArticleCommand(articleId);

        var result = await handler.Handle(command, cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }

    [HttpPut("{articleId}")]
    public async Task<IResult> UpdateArticle(
        [FromRoute] Guid articleId,
        [FromBody] UpdateArticleDto dto,
        [FromServices] ICommandHandler<UpdateArticleCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateArticleCommand(articleId, dto);

        var result = await handler.Handle(command, cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }

    [HttpDelete("{articleId}")]
    public async Task<IResult> DeleteArticle(
       [FromRoute] Guid articleId,
       [FromServices] ICommandHandler<DeleteArticleCommand> handler,
       CancellationToken cancellationToken)
    {
        var command = new DeleteArticleCommand(articleId);

        var result = await handler.Handle(command, cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }
}
