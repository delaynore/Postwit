using ErrorOr;
using Postwit.Application.Abstactions;
using Postwit.DateTimeProvider;
using Postwit.Domain;

namespace Postwit.Application.Articles.Commands.PublishArticle;

public sealed class PublishArticleHandler : ICommandHandler<PublishArticleCommand, Guid>
{
    private readonly IArticleRepository _repository;
    private readonly IUnitOfWork _uof;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PublishArticleHandler(IArticleRepository repository, IUnitOfWork uof, IDateTimeProvider dateTimeProvider)
    {
        _repository = repository;
        _uof = uof;
        _dateTimeProvider = dateTimeProvider;
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
            default, 
            ArticleStatus.Published, 
            _dateTimeProvider);
        
        if (errorOrArticle.IsError)
        {
            return errorOrArticle.Errors;
        }

        await _repository.Articles.AddAsync(errorOrArticle.Value, cancellationToken);
        await _uof.SaveChangesAsync(cancellationToken);

        // send notifications

        return articleId;
    }
}
