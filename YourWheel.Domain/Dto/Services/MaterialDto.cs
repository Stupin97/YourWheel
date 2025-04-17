namespace YourWheel.Domain.Dto.Services
{
    public class MaterialDto
    {
        public Guid MaterialId { get; set; }

        public string? Name { get; set; }

        public double? Price { get; set; }

        public ColorDto Color { get; set; } = null!;

        public FabricatorDto Fabricator { get; set; } = null!;
    }
}
