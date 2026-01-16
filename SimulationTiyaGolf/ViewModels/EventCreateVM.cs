using SimulationTiyaGolf.Models;

namespace SimulationTiyaGolf.ViewModels
{
    public class EventCreateVM
    {
        public DateOnly EventDate { get; set; }
        public IFormFile Image { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int LocationId { get; set; }

        public double Price { get; set; }
    }
}
