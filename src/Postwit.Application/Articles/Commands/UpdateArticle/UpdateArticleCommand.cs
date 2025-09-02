using Postwit.Application.Abstactions;
using Postwit.Application.Contracts.Articles;

namespace Postwit.Application.Articles.Commands.UpdateArticle;

public sealed record UpdateArticleCommand(Guid ArticleId, UpdateArticleDto Dto) : ICommand;
