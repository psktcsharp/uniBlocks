using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("Blocks", Schema = "dbo")]
  public partial class Block
  {
        [NotMapped]
        public int SubsCount {
            get {
                return BlockSubscriptions.Count;
            }
            set { } 
        }
        [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BlockId
    {
      get;
      set;
    }


    public ICollection<BlockUser> BlockUsers { get; set; }

    public ICollection<BlockSubscription> BlockSubscriptions { get; set; }
    public string BlockName
    {
      get;
      set;
    }
    public string location
    {
      get;
      set;
    }
    public bool isActive
    {
      get;
      set;
    }
  }
}
