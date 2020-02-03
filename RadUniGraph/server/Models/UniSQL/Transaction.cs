using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("Transactions", Schema = "dbo")]
  public partial class Transaction
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ATransactionId
    {
      get;
      set;
    }


    public ICollection<Invoice> Invoices { get; set; }
    public string TransactionType
    {
      get;
      set;
    }
    public decimal Amount
    {
      get;
      set;
    }
  }
}
