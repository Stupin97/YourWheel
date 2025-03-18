namespace YourWheel.Domain.Models;

public class AppUser
{
    public Guid AppuserId { get; set; }

    public Guid UserId { get; set; }

    public string? Lastipaddress { get; set; }

    public bool? Isonline { get; set; }

    public DateOnly? Lastconnected { get; set; }

    public DateOnly? Lastdisconected { get; set; }

    public Guid CurrentlanguageId { get; set; }

    public virtual Language Currentlanguage { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
