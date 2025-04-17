namespace YourWheel.Domain.Dto.Orders
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }

        public DateTime DateOrder { get; set; }

        public int Discount { get; set; }

        public DateTime? DateExecute { get; set; }

        public string? Description { get; set; }

        public DateTime? DateEnd { get; set; }

        public PlaceOrderDto? Placeorder { get; set; }

        public StatusDto Status { get; set; } = null!;

        public UserDto User { get; set; } = null!;

        public List<WorkDto> Works { get; set; } = new List<WorkDto>();

        public List<MasterDto> Masters { get; set; } = new List<MasterDto>();
    }
}
