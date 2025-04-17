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

    public async Task<ErrorOr<TagsCollectionResponse>> GetAll(CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.Tags
            .Select(TagsProjections.ToResponse())
            .ToListAsync(cancellationToken);

        return new TagsCollectionResponse(tags);
    }

    public async Task<ErrorOr<TagResponse>> GetById(Guid tagId, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.Tags
            .Where(t => t.Id == tagId)
            .Select(TagsProjections.ToResponse())
            .SingleOrDefaultAsync(cancellationToken);

        if (tag is null)
        {
            return Error.NotFound();
        }

        return tag;
    }

    public async Task<ErrorOr<TagResponse>> CreateTag(CreateTagRequest request, CancellationToken cancellationToken)
    {
        var any = await _tagRepository.Tags.AnyAsync(t => t.Name == request.Name, cancellationToken);

        if (any)
        {
            return Error.Conflict();
        }

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
