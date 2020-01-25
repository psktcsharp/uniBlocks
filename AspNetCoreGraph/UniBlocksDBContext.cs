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

        public DbSet<Service> Services { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}

//UniBlocksDBContext