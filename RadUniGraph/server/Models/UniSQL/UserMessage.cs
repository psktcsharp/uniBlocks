using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("UserMessages", Schema = "dbo")]
  public partial class UserMessage
  {
    [Key]
    public int UserId
    {
      get;
      set;
    }
    public User User { get; set; }
    [Key]
    public int MessageId
    {
      get;
      set;
    }
    public Message Message { get; set; }
  }
}
