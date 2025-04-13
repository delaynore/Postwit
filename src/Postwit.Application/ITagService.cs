using Postwit.Domain;

namespace Postwit.Application;

public interface ITagService
{
    Task<TagResponse> CreateTag(CreateTagRequest request);

}

public record CreateTagRequest(string Name, string Description);

public record TagResponse(Guid Id, string Name, string Description);
