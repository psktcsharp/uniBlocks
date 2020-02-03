using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("BlockSubscriptions", Schema = "dbo")]
  public partial class BlockSubscription
  {
    [Key]
    public int BlockId
    {
      get;
      set;
    }
    public Block Block { get; set; }
    [Key]
    public int SubscriptionId
    {
      get;
      set;
    }
    public Subscription Subscription { get; set; }
  }
}
