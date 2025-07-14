using Postwit.Application.Abstactions;

namespace Postwit.Application.Articles.Commands.PublishDraftedArticle;
public sealed record PublishDraftedArticleCommand(Guid ArticleId) : ICommand;
