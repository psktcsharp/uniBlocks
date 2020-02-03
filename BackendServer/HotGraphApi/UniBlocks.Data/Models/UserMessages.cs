using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class UserMessages
    {
        public int UserId { get; set; }
        public AspNetUser User { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }
    }
}
