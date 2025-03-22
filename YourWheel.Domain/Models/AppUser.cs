namespace YourWheel.Domain.Models;

public class AppUser
{
    public Guid AppUserId { get; set; }

    public Guid UserId { get; set; }

    public string? LastIPAddress { get; set; }

    public bool? IsOnline { get; set; }

    public DateTime? LastConnected { get; set; }

    public DateTime? LastDisconected { get; set; }

    public Guid CurrentLanguageId { get; set; }

    public Language CurrentLanguage { get; set; } = null!;

    public User User { get; set; } = null!;
}
