using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public ICollection<AService> Services { get; } = new List<AService>();
    }
}
