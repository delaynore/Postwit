using ErrorOr;
using Postwit.Application.Abstactions;
using Postwit.DateTimeProvider;
using Postwit.Domain;

namespace Postwit.Application.Articles.Commands.PublishArticle;

public sealed class PublishArticleHandler : ICommandHandler<PublishArticleCommand, Guid>
{
    private readonly IArticlesRepository _repository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ISlugGenerator _slugGenerator;

    public PublishArticleHandler(
        IArticlesRepository repository, 
        IDateTimeProvider dateTimeProvider,
        ISlugGenerator slugGenerator)
    {
        _repository = repository;
        _dateTimeProvider = dateTimeProvider;
        _slugGenerator = slugGenerator;
    }

    public async Task<ErrorOr<Guid>> Handle(PublishArticleCommand command, CancellationToken cancellationToken)
    {
        // remove when users module will be implemented
        var userId = Guid.Parse("02A16E93-F494-4F82-A675-94AC6B64A42B");

        // validate
        var dto = command.ArticleDto;
        var articleId = Guid.CreateVersion7();

        var errorOrArticle = Article.Create(articleId,
            userId,
            dto.Title,
            dto.Content,
            _slugGenerator.GenerateSlug(dto.Title),
            null,
            ArticleStatus.Published,
            _dateTimeProvider);

        if (errorOrArticle.IsError)
        {
            return errorOrArticle.Errors;
        }

        await _repository.AddAsync(errorOrArticle.Value, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        // send notifications

        return articleId;
    }
}
