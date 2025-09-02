using Postwit.Application.Abstactions;
using Postwit.Application.Contracts.Articles;

namespace Postwit.Application.Articles.Commands.PublishArticle;

public sealed record PublishArticleCommand(CreateArticleDto ArticleDto) : ICommand;
