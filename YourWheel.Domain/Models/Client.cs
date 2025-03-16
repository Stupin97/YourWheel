namespace YourWheel.Domain.Models;

public class Client
{
    public Guid ClientId { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;
}
