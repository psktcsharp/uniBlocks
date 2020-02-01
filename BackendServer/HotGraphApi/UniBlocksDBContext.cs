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
           }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //manulay setting many-to-many relation as EF core doesn't support it yet
            //ServiceSubscription
            modelBuilder.Entity<AServiceSubscription>()
           .HasKey(t => new { t.ServiceId, t.SubscriptionId });

            modelBuilder.Entity<AServiceSubscription>()
                .HasOne(pt => pt.Service)
                .WithMany(p => p.AServiceSubscriptions)
                .HasForeignKey(pt => pt.ServiceId);

            modelBuilder.Entity<AServiceSubscription>()
                .HasOne(pt => pt.Subscription)
                .WithMany(t => t.AServiceSubscriptions)
                .HasForeignKey(pt => pt.SubscriptionId);
            //UserBlock
            modelBuilder.Entity<BlockUser>()
         .HasKey(t => new { t.BlockId, t.UserId });

            modelBuilder.Entity<BlockUser>()
                .HasOne(pt => pt.Block)
                .WithMany(p => p.BlockUsers)
                .HasForeignKey(pt => pt.BlockId);

            modelBuilder.Entity<BlockUser>()
                .HasOne(pt => pt.User)
                .WithMany(t => t.BlockUsers)
                .HasForeignKey(pt => pt.UserId);

            //UserMessages
            modelBuilder.Entity<UserMessages>()
     .HasKey(t => new { t.UserId, t.MessageId });

            modelBuilder.Entity<UserMessages>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserMessages)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserMessages>()
                .HasOne(pt => pt.Message)
                .WithMany(t => t.UserMessages)
                .HasForeignKey(pt => pt.MessageId);

        }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<ATransaction> Transactions { get; set; }
        public DbSet<AService> Services { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<AServiceSubscription> AServiceSubscriptions { get; set; }
        public async Task<List<AService>> GetServicesAsync()
        {        
                return await Services.ToListAsync();   
        }
        public async Task<AService> CreateService(AService aservice)
        {
            Services.Add(aservice);
            await SaveChangesAsync();
            return aservice;
        }
        public async Task<Subscription> CreatSubscription(Subscription subscription)
        {
            Subscriptions.Add(subscription);
            await SaveChangesAsync();
            return subscription;
        }
    }
}
