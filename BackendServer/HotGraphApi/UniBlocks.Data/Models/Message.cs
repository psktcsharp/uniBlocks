using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string content { get; set; }
        public User Sender { get; set; }
        public ICollection<User> ToList { get; set; }
    }
}
