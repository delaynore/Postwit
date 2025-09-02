using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Postwit.Application.Abstactions;

namespace Postwit.Application.Articles.Commands.DeleteArticle;

public sealed class DeleteArticleHandler : ICommandHandler<DeleteArticleCommand>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteArticleHandler(IUnitOfWork unitOfWork, IArticleRepository articleRepository)
    {
        _unitOfWork = unitOfWork;
        _articleRepository = articleRepository;
    }

    public async Task<ErrorOr<Success>> Handle(DeleteArticleCommand command, CancellationToken cancellationToken)
    {
        await _articleRepository.Articles.Where(x => x.Id == command.ArticleId).ExecuteDeleteAsync(cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
