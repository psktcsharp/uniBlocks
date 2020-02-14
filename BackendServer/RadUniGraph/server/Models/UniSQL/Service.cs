using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("Services", Schema = "dbo")]
  public partial class Service
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AServiceId
    {
      get;
      set;
    }


    public ICollection<AServiceSubscription> AServiceSubscriptions { get; set; }
    public string ServiceName
    {
      get;
      set;
    }
    public bool IsActive
    {
      get;
      set;
    }
  }
}
