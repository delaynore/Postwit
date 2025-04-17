using Microsoft.AspNetCore.Mvc;
using Postwit.Application;
using Postwit.Application.Contracts.Tags;

namespace Postwit.Api.Controllers;

[ApiController]
[Route("api/tags")]
public sealed class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpPost]
    public async Task<IResult> CreateTag(CreateTagRequest request, CancellationToken cancellationToken)
    {
        var errorOr = await _tagService.CreateTag(request, cancellationToken);

        return errorOr.Match(
            s => Results.Ok(s),
            e => Results.BadRequest(e));
    }

    [HttpPut("{tagId}")]
    public async Task<IResult> CreateTag(Guid tagId, UpdateTagRequest request, CancellationToken cancellationToken)
    {
        var errorOr = await _tagService.UpdateTag(tagId, request, cancellationToken);

        return errorOr.Match(
            s => Results.Ok(s), 
            e => Results.BadRequest(e));
    }
}
