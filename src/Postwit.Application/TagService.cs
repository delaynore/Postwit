using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Postwit.Application.Contracts.Tags;
using Postwit.Application.Mappers;
using Postwit.DateTimeProvider;
using Postwit.Domain;

namespace Postwit.Application;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;


    public TagService(ITagRepository tagRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
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
        var errorOrTag = Tag.Create(
            Guid.CreateVersion7(),
            request.Name,
            request.Description,
            _dateTimeProvider);

        if (errorOrTag.IsError)
        {
            return errorOrTag.Errors;
        }

        var tag = errorOrTag.Value;

        var any = await _tagRepository.Tags.AnyAsync(t => t.Name == tag.Name, cancellationToken);

        if (any)
        {
            return Error.Conflict();
        }

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

        var errorOr = tag.Update(request.Name, request.Description, _dateTimeProvider);

        if (errorOr.IsError)
        {
            return errorOr.Errors;
        }

        var any = await _tagRepository.Tags.AnyAsync(t => t.Name == tag.Name, cancellationToken);

        if (any)
        {
            return Error.Conflict();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return tag.ToResponse();
    }

    public async Task<Deleted> DeleteTag(Guid tagId, CancellationToken cancellationToken)
    {
        await _tagRepository.Tags.Where(t => t.Id == tagId).ExecuteDeleteAsync(cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}
