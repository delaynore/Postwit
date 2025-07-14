using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Postwit.Application.Abstactions;
using Postwit.DateTimeProvider;
using Postwit.Domain;

namespace Postwit.Application.Articles.Commands.PublishDraftedArticle;

public sealed class PublishDraftedArticleCommandHandler : ICommandHandler<PublishDraftedArticleCommand>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PublishDraftedArticleCommandHandler(
        IArticleRepository articleRepository, 
        IUnitOfWork unitOfWork, 
        IDateTimeProvider dateTimeProvider)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }


    public async Task<ErrorOr<Success>> Handle(PublishDraftedArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.Articles
            .FirstOrDefaultAsync(a => a.Id == command.ArticleId, cancellationToken: cancellationToken);

        if (article == null)
        {
            return Error.NotFound();
        }

        if (article.Status == ArticleStatus.Published)
        {
            return Result.Success;
        }

        if (article.Status != ArticleStatus.Drafted)
        {
            return Error.Failure("not existed status");
        }
        
        article.Status = ArticleStatus.Published;
        article.UpdatedAtUtc = _dateTimeProvider.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
