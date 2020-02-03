using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("Invoice", Schema = "dbo")]
  public partial class Invoice
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InvoiceId
    {
      get;
      set;
    }
    public int? AServiceSubscriptionServiceId
    {
      get;
      set;
    }
    public AServiceSubscription AServiceSubscription { get; set; }
    public int? AServiceSubscriptionSubscriptionId
    {
      get;
      set;
    }
    public int? TransactionATransactionId
    {
      get;
      set;
    }
    public Transaction Transaction { get; set; }
  }
}
