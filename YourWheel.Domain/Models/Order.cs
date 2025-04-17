namespace YourWheel.Domain.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public DateTime DateOrder { get; set; }

        public Guid StatusId { get; set; }

        public int Discount { get; set; }

        public Guid UserId { get; set; }

        public DateTime? DateExecute { get; set; }

        public Guid? PlaceOrderId { get; set; }

        public string? Description { get; set; }

        public DateTime? DateEnd { get; set; }

        public Guid? MajorId { get; set; }

        public List<Order> InverseMajor { get; set; } = new List<Order>();

        public Order? Major { get; set; }

        public PlaceOrder? PlaceOrder { get; set; }

        public Status Status { get; set; } = null!;

        public User User { get; set; } = null!;

        public List<Work> Works { get; set; } = new List<Work>();

        public List<Master> Masters { get; set; } = new List<Master>();
    }
}
