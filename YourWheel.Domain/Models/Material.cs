namespace YourWheel.Domain.Models
{
    public class Material
    {
        public Guid MaterialId { get; set; }

        public string? Name { get; set; }

        public double? Price { get; set; }

        public Guid SupplierId { get; set; }

        public Guid FabricatorId { get; set; }

        public Guid ColorId { get; set; }

        public Color Color { get; set; } = null!;

        public Fabricator Fabricator { get; set; } = null!;

        public Supplier Supplier { get; set; } = null!;

        public List<Work> Works { get; set; } = new List<Work>();
    }
}
