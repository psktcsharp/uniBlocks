using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using UniBlocksGraph.Models.UniSql;

namespace UniBlocksGraph.Data
{
  public partial class UniSqlContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public UniSqlContext(DbContextOptions<UniSqlContext> options):base(options)
    {
    }

    public UniSqlContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UniBlocksGraph.Models.UniSql.AServiceSubscription>().HasKey(table => new {
          table.ServiceId, table.SubscriptionId
        });
        builder.Entity<UniBlocksGraph.Models.UniSql.BlockSubscription>().HasKey(table => new {
          table.BlockId, table.SubscriptionId
        });
        builder.Entity<UniBlocksGraph.Models.UniSql.BlockUser>().HasKey(table => new {
          table.BlockId, table.UserId
        });
        builder.Entity<UniBlocksGraph.Models.UniSql.UserMessage>().HasKey(table => new {
          table.UserId, table.MessageId
        });
        builder.Entity<UniBlocksGraph.Models.UniSql.AServiceSubscription>()
              .HasOne(i => i.Service)
              .WithMany(i => i.AServiceSubscriptions)
              .HasForeignKey(i => i.ServiceId)
              .HasPrincipalKey(i => i.AServiceId);
        builder.Entity<UniBlocksGraph.Models.UniSql.AServiceSubscription>()
              .HasOne(i => i.Subscription)
              .WithMany(i => i.AServiceSubscriptions)
              .HasForeignKey(i => i.SubscriptionId)
              .HasPrincipalKey(i => i.SubscriptionId);
        builder.Entity<UniBlocksGraph.Models.UniSql.BlockSubscription>()
              .HasOne(i => i.Block)
              .WithMany(i => i.BlockSubscriptions)
              .HasForeignKey(i => i.BlockId)
              .HasPrincipalKey(i => i.BlockId);
        builder.Entity<UniBlocksGraph.Models.UniSql.BlockSubscription>()
              .HasOne(i => i.Subscription)
              .WithMany(i => i.BlockSubscriptions)
              .HasForeignKey(i => i.SubscriptionId)
              .HasPrincipalKey(i => i.SubscriptionId);
        builder.Entity<UniBlocksGraph.Models.UniSql.BlockUser>()
              .HasOne(i => i.Block)
              .WithMany(i => i.BlockUsers)
              .HasForeignKey(i => i.BlockId)
              .HasPrincipalKey(i => i.BlockId);
        builder.Entity<UniBlocksGraph.Models.UniSql.BlockUser>()
              .HasOne(i => i.User)
              .WithMany(i => i.BlockUsers)
              .HasForeignKey(i => i.UserId)
              .HasPrincipalKey(i => i.UserId);
        builder.Entity<UniBlocksGraph.Models.UniSql.Invoice>()
              .HasOne(i => i.AServiceSubscription)
              .WithMany(i => i.Invoices)
              .HasForeignKey(i => new { i.AServiceSubscriptionServiceId, i.AServiceSubscriptionSubscriptionId })
              .HasPrincipalKey(i => new { i.ServiceId, i.SubscriptionId });
        builder.Entity<UniBlocksGraph.Models.UniSql.Invoice>()
              .HasOne(i => i.Transaction)
              .WithMany(i => i.Invoices)
              .HasForeignKey(i => i.TransactionATransactionId)
              .HasPrincipalKey(i => i.ATransactionId);
        builder.Entity<UniBlocksGraph.Models.UniSql.Message>()
              .HasOne(i => i.User)
              .WithMany(i => i.Messages)
              .HasForeignKey(i => i.SenderUserId)
              .HasPrincipalKey(i => i.UserId);

        builder.Entity<UniBlocksGraph.Models.UniSql.Subscription>()
              .HasOne(i => i.User)
              .WithMany(i => i.Subscriptions)
              .HasForeignKey(i => i.UserId)
              .HasPrincipalKey(i => i.UserId);
        builder.Entity<UniBlocksGraph.Models.UniSql.UserMessage>()
              .HasOne(i => i.User)
              .WithMany(i => i.UserMessages)
              .HasForeignKey(i => i.UserId)
              .HasPrincipalKey(i => i.UserId);
        builder.Entity<UniBlocksGraph.Models.UniSql.UserMessage>()
              .HasOne(i => i.Message)
              .WithMany(i => i.UserMessages)
              .HasForeignKey(i => i.MessageId)
              .HasPrincipalKey(i => i.MessageId);


        this.OnModelBuilding(builder);
    }


    public DbSet<UniBlocksGraph.Models.UniSql.AServiceSubscription> AServiceSubscriptions
    {
      get;
      set;
    }

  

    public DbSet<UniBlocksGraph.Models.UniSql.Block> Blocks
    {
      get;
      set;
    }

    public DbSet<UniBlocksGraph.Models.UniSql.BlockSubscription> BlockSubscriptions
    {
      get;
      set;
    }

    public DbSet<UniBlocksGraph.Models.UniSql.BlockUser> BlockUsers
    {
      get;
      set;
    }

    public DbSet<UniBlocksGraph.Models.UniSql.Invoice> Invoices
    {
      get;
      set;
    }

    public DbSet<UniBlocksGraph.Models.UniSql.Message> Messages
    {
      get;
      set;
    }

    public DbSet<UniBlocksGraph.Models.UniSql.Service> Services
    {
      get;
      set;
    }

    public DbSet<UniBlocksGraph.Models.UniSql.Subscription> Subscriptions
    {
      get;
      set;
    }

    public DbSet<UniBlocksGraph.Models.UniSql.Transaction> Transactions
    {
      get;
      set;
    }

    public DbSet<UniBlocksGraph.Models.UniSql.User> Users
    {
      get;
      set;
    }

    public DbSet<UniBlocksGraph.Models.UniSql.UserMessage> UserMessages
    {
      get;
      set;
    }
  }
}
