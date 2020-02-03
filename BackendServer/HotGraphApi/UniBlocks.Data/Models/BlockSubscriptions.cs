using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class BlockSubscriptions
    {
        public int BlockId { get; set; }
        public Block Block { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
    }
}
