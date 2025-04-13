namespace YourWheel.Domain.Models
{
    public class Status
    {
        public Guid StatusId { get; set; }

        public string Name { get; set; } = null!;

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
