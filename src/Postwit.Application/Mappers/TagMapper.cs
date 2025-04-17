using Postwit.Application.Contracts.Tags;
using Postwit.Domain;

namespace Postwit.Application.Mappers;

internal static class TagMapper
{
    public static TagResponse ToResponse(this Tag tag)
    {
        return new(tag.Id, tag.Name, tag.Description, tag.CreatedAtUtc, tag.UpdatedAtUtc);
    }
}
