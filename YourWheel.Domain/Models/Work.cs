namespace YourWheel.Domain.Models
{
    public class Work
    {
        public Guid WorkId { get; set; }

        public Guid OrderId { get; set; }

        public Guid? ProductId { get; set; }

        public Guid? TypeWorkId { get; set; }

        public Guid? MaterialId { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public Guid? WorkPlaceId { get; set; }

        public Material? Material { get; set; }

        public Order Order { get; set; } = null!;

        public TypeWork? Typework { get; set; }
    }
}
