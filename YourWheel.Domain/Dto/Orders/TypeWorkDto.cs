namespace YourWheel.Domain.Dto.Orders
{
    public class TypeWorkDto
    {
        public Guid TypeworkId { get; set; }

        public string Name { get; set; } = null!;

        public double Price { get; set; }
    }
}
