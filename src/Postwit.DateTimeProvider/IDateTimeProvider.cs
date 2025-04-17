namespace Postwit.DateTimeProvider;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

