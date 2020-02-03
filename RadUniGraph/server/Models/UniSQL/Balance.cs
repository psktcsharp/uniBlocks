using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("Balances", Schema = "dbo")]
  public partial class Balance
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BalanceId
    {
      get;
      set;
    }


    public ICollection<Subscription> Subscriptions { get; set; }
    public decimal value
    {
      get;
      set;
    }
  }
}
