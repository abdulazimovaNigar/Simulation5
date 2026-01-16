using SimulationTiyaGolf.Common;

namespace SimulationTiyaGolf.Models
{
    public class Location : BaseEntity
    {
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
