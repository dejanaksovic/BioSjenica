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
    }
}
