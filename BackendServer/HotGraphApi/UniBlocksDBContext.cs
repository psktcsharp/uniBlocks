using HotGraphApi.UniBlocks.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi
{
    public class UniBlocksDBContext : DbContext
    {


        public UniBlocksDBContext(DbContextOptions<UniBlocksDBContext> options)
    : base(options)

        {
            // Drop the database if it exists
           }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //    optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=UniBlocksDB;Integrated Security=True");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
          

            base.OnModelCreating(modelBuilder);

            //manulay setting many-to-many relation as EF core doesn't support it yet
            modelBuilder.Entity<AServiceSubscription>()
         .HasKey(s => new { s.ServiceId, s.SubscriptionId });
        }
        public DbSet<AService> Services { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
    }
}
