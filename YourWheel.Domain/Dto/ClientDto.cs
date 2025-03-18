namespace YourWheel.Domain.Dto
{
    /// <summary>
    /// Dto для клиента
    /// </summary>
    public class UserDto
    {
        public Guid UserId { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
