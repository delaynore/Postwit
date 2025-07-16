using Microsoft.AspNetCore.Mvc;
using Postwit.Application.Abstactions;
using Postwit.Application.Articles.Commands.PublishArticle;
using Postwit.Application.Articles.Commands.PublishDraftedArticle;
using Postwit.Application.Articles.Commands.UpdateArticle;
using Postwit.Application.Contracts.Articles;

namespace Postwit.Api.Controllers;

[ApiController]
[Route("api/articles")]
public sealed class ArticlesController : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PublishArticle(
        [FromBody] CreateArticleDto dto,
        [FromServices] ICommandHandler<PublishArticleCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new PublishArticleCommand(dto), cancellationToken);

        return result.Match(
             s => Results.Ok(s),
             e => Results.BadRequest(e));
    }

    [HttpPut("{articleId}/publish")]
    public async Task<IResult> PublishDraftedArticle(
        [FromRoute] Guid articleId,
        [FromServices] ICommandHandler<PublishDraftedArticleCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new PublishDraftedArticleCommand(articleId);

        var result = await handler.Handle(command, cancellationToken);

        return result.Match(
            s => Results.Ok(),
            e => Results.BadRequest(e));
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

        return result.Match(
            s => Results.Ok(),
            e => Results.BadRequest(e));
    }
}
