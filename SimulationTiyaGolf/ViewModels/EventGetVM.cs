using SimulationTiyaGolf.Models;

namespace SimulationTiyaGolf.ViewModels
{
    public class EventGetVM
    {
        public int Id { get; set; }
        public DateOnly EventDate { get; set; }
        public string Image { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string LocationCountry { get; set; } = string.Empty;
        public string LocationCity { get; set; } = string.Empty;


        public double Price { get; set; }
    }
}
