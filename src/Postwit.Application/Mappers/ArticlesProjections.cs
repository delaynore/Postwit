using System.Linq.Expressions;
using Postwit.Application.Contracts.Articles;
using Postwit.Domain;

namespace Postwit.Application.Mappers;

internal static class ArticlesProjections
{
    public static Expression<Func<Article, ArticleDto>> ToDto()
    {
        return a => new(a.Id, a.Title, a.Content, a.AuthorId, a.ReadTime, a.CreatedAtUtc, a.TagsIds);
    }
}

