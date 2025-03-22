namespace YourWheel.Domain.Dto
{
    /// <summary>
    /// Dto собранной не личной информации по пользователю
    /// </summary>
    public class AppUserDto
    {
        public Guid AppUserId { get; set; }

        public Guid UserId { get; set; }

        public string? LastIPAddress { get; set; }

        public bool? IsOnline { get; set; }

        public DateTime? LastConnected { get; set; }

        public DateTime? LastDisconected { get; set; }

        public Guid CurrentLanguageId { get; set; }
    }
}
