namespace Postwit.Domain;

public sealed class User
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string DisplayName { get; set; }

    public string AvatarUrl { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
