namespace YourWheel.Domain.Dto.Orders
{
    public class PlaceOrderDto
    {
        public Guid PlaceorderId { get; set; }

        public string? Name { get; set; }

        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}
