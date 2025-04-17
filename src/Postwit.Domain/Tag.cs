using ErrorOr;
using Postwit.DateTimeProvider;

namespace Postwit.Domain;

public sealed class Tag
{
    public const int MinNameLength = 2;
    public const int MaxNameLength = 64;
    public const int MaxDescriptionLength = 512;

    public required Guid Id { get; init; }

    public required string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    public required DateTime CreatedAtUtc {  get; init; }

    public DateTime? UpdatedAtUtc { get; set; }

    private Tag()
    { }

    public static ErrorOr<Tag> Create(Guid id, string name, string description, IDateTimeProvider dateTimeProvider)
    {
        var errorOrSuccess = Validate(name, description);

        if (errorOrSuccess.IsError)
        {
            return errorOrSuccess.Errors;
        }

        return new Tag()
        {
            Id = id,
            Name = name,
            Description = description,
            CreatedAtUtc = dateTimeProvider.UtcNow,
        };
    }

    public ErrorOr<Updated> Update(string name, string description, IDateTimeProvider dateTimeProvider)
    {
        var errorOrSuccess = Validate(name, description);

        if (errorOrSuccess.IsError)
        {
            return errorOrSuccess.Errors;
        }

        Name = name;
        Description = description;
        UpdatedAtUtc = dateTimeProvider.UtcNow;

        return Result.Updated;
    }

    private static ErrorOr<Success> Validate(string name, string description)
    {
        List<Error> errors = [];

        if (string.IsNullOrEmpty(name))
        {
            errors.Add(Error.Validation(code: "Tag.Name", description: "Must be non empty string"));
        }
        else if (name is { Length: > MaxNameLength or < MinNameLength })
        {
            errors.Add(Error.Validation(code: "Tag.Name",
                description: $"Length of string must be between {MinNameLength} and {MaxNameLength}"));
        }

        if (description is { Length: > MaxDescriptionLength })
        {
            errors.Add(Error.Validation(code: "Tag.Description",
                description: $"Max length of description is {MaxDescriptionLength}"));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return Result.Success;
    }
}
