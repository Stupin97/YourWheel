namespace YourWheel.Domain.Models;

public class User
{
    public Guid UserId { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public Guid RoleId { get; set; }

    public string? Email { get; set; }

    public List<AppUser> AppUsers { get; set; } = new List<AppUser>();

    public Role Role { get; set; } = null!;

    public List<Car> Cars { get; set; } = new List<Car>();
}
