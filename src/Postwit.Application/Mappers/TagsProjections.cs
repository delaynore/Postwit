using System.Linq.Expressions;
using Postwit.Application.Contracts.Tags;
using Postwit.Domain;

namespace Postwit.Application.Mappers;

internal static class TagsProjections
{
    public static Expression<Func<Tag, TagResponse>> ToResponse()
    {
        return t => new TagResponse(t.Id, t.Name, t.Description, t.CreatedAtUtc, t.UpdatedAtUtc);
    }
}

