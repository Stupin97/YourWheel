namespace YourWheel.Domain.Models
{
    public class Master
    {
        public Guid MasterId { get; set; }

        public Guid UserId { get; set; }

        public DateTime? WorkExperienceDate { get; set; }

        public Guid? PositionId { get; set; }

        public Position? Position { get; set; }

        public User User { get; set; } = null!;

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
