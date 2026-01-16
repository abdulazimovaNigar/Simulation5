using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SimulationTiyaGolf.Models;

namespace SimulationTiyaGolf.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Models.Location> Locations { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
    }
}
