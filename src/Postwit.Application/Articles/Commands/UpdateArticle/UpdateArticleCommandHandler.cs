using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Postwit.Application.Abstactions;
using Postwit.DateTimeProvider;

namespace Postwit.Application.Articles.Commands.UpdateArticle;

public sealed class UpdateArticleCommandHandler : ICommandHandler<UpdateArticleCommand>
{
    private readonly IArticlesRepository _articlesRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateArticleCommandHandler(
        IArticlesRepository articlesRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _articlesRepository = articlesRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _articlesRepository.GetByIdAsync(command.ArticleId, cancellationToken);

        if (article == null)
        {
            return Error.NotFound("article.not_found");
        }

        article.Title = command.Dto.Title;
        article.Content = command.Dto.Content;
        //update read time
        //update slug
        article.UpdatedAtUtc = _dateTimeProvider.UtcNow;

        await _articlesRepository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
