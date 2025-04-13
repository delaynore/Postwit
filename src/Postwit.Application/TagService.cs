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

    public async Task<TagResponse> CreateTag(CreateTagRequest request)
    {
        var tag = request.ToEntity();
        tag.CreatedAtUtc = DateTime.UtcNow;

        _tagRepository.Tags.Add(tag);

        await _unitOfWork.SaveChangesAsync(default);

        return tag.ToResponse();
    }
}
