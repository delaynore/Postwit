using Postwit.Application.Contracts.Tags;
using Postwit.Domain;

namespace Postwit.Application;

public interface ITagService
{
    Task<TagResponse> CreateTag(CreateTagRequest request, CancellationToken cancellationToken);

}
