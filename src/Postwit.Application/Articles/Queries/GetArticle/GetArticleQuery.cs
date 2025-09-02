using Postwit.Application.Abstactions;

namespace Postwit.Application.Articles.Queries.GetArticle;

public sealed record GetArticleQuery(string IdOrSlug) : IQuery;
