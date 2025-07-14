using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Postwit.Application.Abstactions;
using Postwit.Application.Contracts.Articles;
using Postwit.DateTimeProvider;

namespace Postwit.Application.Articles.Commands.UpdateArticle;

public sealed record UpdateArticleCommand(Guid ArticleId, UpdateArticleDto Dto) : ICommand;

public sealed class UpdateArticleCommandHandler : ICommandHandler<UpdateArticleCommand>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateArticleCommandHandler(
        IArticleRepository articleRepository, 
        IUnitOfWork unitOfWork, 
        IDateTimeProvider dateTimeProvider)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.Articles.
            FirstOrDefaultAsync(a => a.Id == command.ArticleId, cancellationToken);

        if (article == null)
        {
            return Error.NotFound();
        }

        article.Title = command.Dto.Title;
        article.Content = command.Dto.Content;
        //update read time
        //update slug
        article.UpdatedAtUtc = _dateTimeProvider.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
