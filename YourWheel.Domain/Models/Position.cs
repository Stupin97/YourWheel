namespace YourWheel.Domain.Models
{
    public class Position
    {
        public Guid PositionId { get; set; }

        public string Name { get; set; } = null!;

        public List<Master> Masters { get; set; } = new List<Master>();
    }
}
