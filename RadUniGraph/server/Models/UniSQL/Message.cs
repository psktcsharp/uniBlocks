using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("Messages", Schema = "dbo")]
  public partial class Message
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MessageId
    {
      get;
      set;
    }


    public ICollection<UserMessage> UserMessages { get; set; }
    public string content
    {
      get;
      set;
    }
    public int? SenderUserId
    {
      get;
      set;
    }
    public User User { get; set; }
  }
}
