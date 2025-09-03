using ErrorOr;
using Postwit.Application.Abstactions;
using Postwit.DateTimeProvider;
using Postwit.Domain;

namespace Postwit.Application.Articles.Commands.PublishDraftedArticle;

public sealed class PublishDraftedArticleCommandHandler : ICommandHandler<PublishDraftedArticleCommand>
{
    private readonly IArticlesRepository _repository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PublishDraftedArticleCommandHandler(
        IArticlesRepository repository,
        IDateTimeProvider dateTimeProvider)
    {
        _repository = repository;
        _dateTimeProvider = dateTimeProvider;
    }


    public async Task<ErrorOr<Success>> Handle(PublishDraftedArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _repository.GetByIdAsync(command.ArticleId, cancellationToken);

        if (article is null)
        {
            return Error.NotFound();
        }

        if (article.Status is ArticleStatus.Published)
        {
            return Result.Success;
        }

        if (article.Status is not ArticleStatus.Drafted)
        {
            return Error.Failure("article.invalid_status");
        }

        article.Status = ArticleStatus.Published;
        article.UpdatedAtUtc = _dateTimeProvider.UtcNow;

        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
