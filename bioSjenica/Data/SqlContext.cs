using bioSjenica.Models;
using Microsoft.EntityFrameworkCore;

namespace bioSjenica.Data
{
    public class SqlContext:DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options):base(options)
        {
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<FeedingGround> FeedingGorunds{ get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<User> Workers{ get; set; }
    }
}
