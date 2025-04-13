namespace YourWheel.Domain.Models
{
    public class Supplier
    {
        public Guid SupplierId { get; set; }

        public string Name { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Url { get; set; }

        public List<Material> Materials { get; set; } = new List<Material>();
    }
}
