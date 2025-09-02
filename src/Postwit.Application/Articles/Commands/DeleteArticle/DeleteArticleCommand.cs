using Postwit.Application.Abstactions;

namespace Postwit.Application.Articles.Commands.DeleteArticle;

public sealed record DeleteArticleCommand(Guid ArticleId) : ICommand;
