namespace YourWheel.Domain.Models
{
    public class PlaceOrder
    {
        public Guid PlaceorderId { get; set; }

        public string? Name { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
