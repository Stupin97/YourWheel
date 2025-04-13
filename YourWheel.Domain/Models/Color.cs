namespace YourWheel.Domain.Models
{
    public class Color
    {
        public Guid ColorId { get; set; }

        public string? Name { get; set; }

        public List<Material> Materials { get; set; } = new List<Material>();
    }
}
