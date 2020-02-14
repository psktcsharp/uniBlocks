using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("AServiceSubscriptions", Schema = "dbo")]
  public partial class AServiceSubscription
  {
    [Key]
    public int ServiceId
    {
      get;
      set;
    }

    public ICollection<Invoice> Invoices { get; set; }
    public Service Service { get; set; }
    [Key]
    public int SubscriptionId
    {
      get;
      set;
    }
    public Subscription Subscription { get; set; }
  }
}
