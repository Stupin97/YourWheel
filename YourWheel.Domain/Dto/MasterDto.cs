namespace YourWheel.Domain.Dto
{
    using YourWheel.Domain.Dto.Orders;

    public class MasterDto
    {
        public Guid MasterId { get; set; }

        public DateTime? WorkExperienceDate { get; set; }

        public PositionDto? Position { get; set; }

        public UserDto User { get; set; } = null!;

        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}
