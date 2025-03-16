namespace YourWheel.Domain.Dto
{
    /// <summary>
    /// Dto для клиента
    /// </summary>
    public class ClientDto
    {
        public Guid ClientId { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
