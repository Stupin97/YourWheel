namespace YourWheel.Domain.Models;

public class Language
{
    public Guid LanguageId { get; set; }

    public string? Name { get; set; }

    public List<AppUser> AppUsers { get; set; } = new List<AppUser>();
}
