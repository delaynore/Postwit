using ErrorOr;
using Postwit.Application.Contracts.Tags;
using Postwit.Domain;

namespace Postwit.Application;

public interface ITagService
{
    Task<ErrorOr<TagResponse>> GetById(Guid tagId, CancellationToken cancellationToken);

    Task<ErrorOr<TagsCollectionResponse>> GetAll(CancellationToken cancellationToken);

    Task<ErrorOr<TagResponse>> CreateTag(CreateTagRequest request, CancellationToken cancellationToken);

    Task<ErrorOr<TagResponse>> UpdateTag(Guid tagId, UpdateTagRequest request, CancellationToken cancellationToken);

    Task<Deleted> DeleteTag(Guid tagId, CancellationToken cancellationToken);
}
