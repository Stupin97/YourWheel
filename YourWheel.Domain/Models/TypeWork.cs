namespace YourWheel.Domain.Models
{
    public class TypeWork
    {
        public Guid TypeworkId { get; set; }

        public string Name { get; set; } = null!;

        public double Price { get; set; }

        public List<Work> Works { get; set; } = new List<Work>();
    }
}
