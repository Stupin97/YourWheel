namespace YourWheel.Domain.Models;

public class Car
{
    public Guid CarId { get; set; }

    public string? Namemark { get; set; }

    public List<User> Users { get; set; } = new List<User>();
}
