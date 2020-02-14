using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("Subscriptions", Schema = "dbo")]
  public partial class Subscription
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SubscriptionId
    {
      get;
      set;
    }


    public ICollection<AServiceSubscription> AServiceSubscriptions { get; set; }

    public ICollection<BlockSubscription> BlockSubscriptions { get; set; }
    public string SubscriptionName
    {
      get;
      set;
    }

   
    public int UserId
    {
      get;
      set;
    }
    public User User { get; set; }
    public bool IsActive
    {
      get;
      set;
    }
  }
}
