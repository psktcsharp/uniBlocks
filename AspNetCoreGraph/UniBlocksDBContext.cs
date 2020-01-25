using AspNetCoreGraph.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreGraph
{

    public class UniBlocksDBContext : DbContext
    {
        public UniBlocksDBContext() { }
        public UniBlocksDBContext(DbContextOptions<UniBlocksDBContext> options)
          : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
        }

        public DbSet<Service> Books { get; set; }
        public DbSet<Subscription> Authors { get; set; }
    }
}

//UniBlocksDBContext