using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Postwit.Application.Contracts.Tags;
using Postwit.Application.Mappers;
using Postwit.Domain;

namespace Postwit.Application;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;


    public TagService(ITagRepository tagRepository, IUnitOfWork unitOfWork)
    {
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TagResponse> CreateTag(CreateTagRequest request, CancellationToken cancellationToken)
    {
        var tag = request.ToEntity();
        tag.CreatedAtUtc = DateTime.UtcNow;

        _tagRepository.Tags.Add(tag);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return tag.ToResponse();
    }

    public async Task<ErrorOr<TagResponse>> UpdateTag(Guid tagId, UpdateTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.Tags.SingleOrDefaultAsync(t => t.Id == tagId, cancellationToken);

        if (tag is null)
        {
            return Error.NotFound();
        }

        var any = await _tagRepository.Tags.AnyAsync(t => t.Name == request.Name, cancellationToken);

        if (any)
        {
            return Error.Conflict();
        }

        tag.Name = request.Name;
        tag.Description = request.Description;
        tag.UpdatedAtUtc = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return tag.ToResponse();   
    }
}
