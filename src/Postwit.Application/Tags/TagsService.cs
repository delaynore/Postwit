using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Postwit.Application.Contracts.Tags;
using Postwit.Application.Mappers;
using Postwit.DateTimeProvider;
using Postwit.Domain;

namespace Postwit.Application.Tags;

public class TagsService : ITagsService
{
    private readonly ITagsRepository _tagsRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public TagsService(
        ITagsRepository tagsRepository,
        IReadDbContext readDbContext, 
        IDateTimeProvider dateTimeProvider)
    {
        _tagsRepository = tagsRepository;
        _readDbContext = readDbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<TagsCollectionResponse>> GetAll(CancellationToken cancellationToken)
    {
        var tags = await _readDbContext.ReadTags
            .Select(TagsProjections.ToResponse())
            .ToListAsync(cancellationToken);

        return new TagsCollectionResponse(tags);
    }

    public async Task<ErrorOr<TagResponse>> GetById(Guid tagId, CancellationToken cancellationToken)
    {
        var tag = await _readDbContext.ReadTags
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

        var existedTag = await _tagsRepository.GetByNameAsync(tag.Name, cancellationToken);

        if (existedTag is not null)
        {
            return Error.Conflict();
        }

        await _tagsRepository.AddAsync(tag, cancellationToken);

        await _tagsRepository.SaveChangesAsync(cancellationToken);

        return tag.ToResponse();
    }

    public async Task<ErrorOr<TagResponse>> UpdateTag(Guid tagId, UpdateTagRequest request, CancellationToken cancellationToken)
    {
        var tag = await _tagsRepository.GetByIdAsync(tagId, cancellationToken);

        if (tag is null)
        {
            return Error.NotFound();
        }

        var errorOr = tag.Update(request.Name, request.Description, _dateTimeProvider);

        if (errorOr.IsError)
        {
            return errorOr.Errors;
        }

        var existedTag = await _tagsRepository.GetByNameAsync(tag.Name, cancellationToken);

        if (existedTag is not null)
        {
            return Error.Conflict();
        }

        await _tagsRepository.SaveChangesAsync(cancellationToken);

        return tag.ToResponse();
    }

    public async Task<Deleted> DeleteTag(Guid tagId, CancellationToken cancellationToken)
    {
        await _tagsRepository.DeleteAsync(tagId, cancellationToken);

        await _tagsRepository.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}
