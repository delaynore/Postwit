using ErrorOr;
using Postwit.Application.Abstactions;

namespace Postwit.Application.Articles.Commands.DeleteArticle;

public sealed class DeleteArticleHandler : ICommandHandler<DeleteArticleCommand>
{
    private readonly IArticlesRepository _repository;

    public DeleteArticleHandler(IArticlesRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Success>> Handle(DeleteArticleCommand command, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(command.ArticleId, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
