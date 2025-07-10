using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Postwit.Application.Abstactions;
using Postwit.Application.Articles.Commands.PublishArticle;
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
             e => Results.NotFound(e));
    }
}
