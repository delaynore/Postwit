using Microsoft.AspNetCore.Mvc;
using Postwit.Application;

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
    public async Task<IResult> CreateTag(CreateTagRequest request)
    {
        var tag = await _tagService.CreateTag(request);

        return Results.Ok(tag);
    }
}
