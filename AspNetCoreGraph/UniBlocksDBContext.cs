using AspNetCoreGraph.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Database
{

    public class UniBlocksDBContext : DbContext
    {
        public UniBlocksDBContext() { }
        public UniBlocksDBContext(DbContextOptions<UniBlocksDBContext> options)
          : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInternalServiceProvider("UniBlocksDB");
        }

        public DbSet<Service> Books { get; set; }
        public DbSet<Subscription> Authors { get; set; }
    }
}

//UniBlocksDBContext