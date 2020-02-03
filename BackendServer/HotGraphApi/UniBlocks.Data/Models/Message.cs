using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class Message
    {
        public Message()
        {
            UserMessages = new List<UserMessages>();
        }
        public int MessageId { get; set; }
        public string content { get; set; }
        public AspNetUser Sender { get; set; }
        public ICollection<UserMessages> UserMessages { get; set; }
    }
}
