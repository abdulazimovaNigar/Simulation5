using SimulationTiyaGolf.Common;

namespace SimulationTiyaGolf.Models
{
    public class Event : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly EventDate { get; set; }
        public string Image { get; set; } = null!;
        public Location Location { get; set; } = null!; 
        public int LocationId { get; set; }

        public double Price { get; set; }
    }
}
