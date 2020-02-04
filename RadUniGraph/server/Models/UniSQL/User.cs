using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniBlocksGraph.Models.UniSql
{
  [Table("Users", Schema = "dbo")]
  public partial class User
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId
    {
      get;
      set;
    }


    public ICollection<BlockUser> BlockUsers { get; set; }

    public ICollection<Message> Messages { get; set; }

    public ICollection<Subscription> Subscriptions { get; set; }

    public ICollection<UserMessage> UserMessages { get; set; }
    public string Email
    {
      get;
      set;
    }
    public string Password
    {
      get;
      set;
    }
    public bool isUser
    {
      get;
      set;
    }
    public bool isAdmin
    {
      get;
      set;
    }
    public string PhoneNumber
    {
      get;
      set;
    }
        public string AspNetId { get; set; }
    }
}
