namespace YourWheel.Domain.Dto.Orders
{
    using YourWheel.Domain.Dto.Services;

    public class WorkDto
    {
        public Guid WorkId { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public MaterialDto? Material { get; set; }

        public TypeWorkDto? Typework { get; set; }
    }
}
