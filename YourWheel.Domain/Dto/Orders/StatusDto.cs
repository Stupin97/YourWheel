namespace YourWheel.Domain.Dto.Orders
{
    public class StatusDto
    {
        public Guid StatusId { get; set; }

        public string Name { get; set; } = null!;

        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}
