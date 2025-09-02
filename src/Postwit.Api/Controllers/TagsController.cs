using ErrorOr;
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

    [HttpGet("{tagId}")]
    public async Task<IResult> GetById(Guid tagId, CancellationToken cancellationToken)
    {
        var result = await _tagService.GetById(tagId, cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }

    [HttpGet]
    public async Task<IResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _tagService.GetAll(cancellationToken);
        
        return result.Match(Results.Ok, Results.NotFound);
    }

    [HttpPost]
    public async Task<IResult> CreateTag(CreateTagRequest request, CancellationToken cancellationToken)
    {
        var result = await _tagService.CreateTag(request, cancellationToken);

        return result.Match(Results.Ok, Results.BadRequest);
    }

    [HttpPut("{tagId}")]
    public async Task<IResult> CreateTag(Guid tagId, UpdateTagRequest request, CancellationToken cancellationToken)
    {
        var result = await _tagService.UpdateTag(tagId, request, cancellationToken);

        return result.Match(
            s => Results.Ok(s), 
            e => Results.BadRequest(e));
    }

    [HttpDelete("{tagId}")]
    public async Task<IResult> DeleteTag(Guid tagId, CancellationToken cancellationToken)
    {
        await _tagService.DeleteTag(tagId, cancellationToken);
        
        return Results.NoContent();
    }
}
