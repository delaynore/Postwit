using Postwit.Application.Abstactions;
using Postwit.DateTimeProvider;

namespace Postwit.Application.Articles.Queries.GetArticle;
public sealed record GetArticleQuery(string IdOrSlug) : IQuery;
