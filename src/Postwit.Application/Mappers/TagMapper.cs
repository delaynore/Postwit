using Postwit.Domain;

namespace Postwit.Application.Mappers;

internal static class TagMapper
{
    public static Tag ToEntity(this CreateTagRequest tag)
    {
        return new Tag()
        {
            Name = tag.Name,
            Description = tag.Description,
        };
    }

    public static TagResponse ToResponse(this Tag tag)
    {
        return new(tag.Id, tag.Name, tag.Description);
    }
}
