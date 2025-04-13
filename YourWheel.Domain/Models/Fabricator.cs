namespace YourWheel.Domain.Models
{
    public class Fabricator
    {
        public Guid FabricatorId { get; set; }

        public string? Name { get; set; }

        public string? Country { get; set; }

        public List<Material> Materials { get; set; } = new List<Material>();
    }
}
