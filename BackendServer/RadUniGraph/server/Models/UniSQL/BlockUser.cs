using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("BlockUser", Schema = "dbo")]
  public partial class BlockUser
  {
    [Key]
    public int BlockId
    {
      get;
      set;
    }
    public Block Block { get; set; }
    [Key]
    public int UserId
    {
      get;
      set;
    }
    public User User { get; set; }
  }
}
