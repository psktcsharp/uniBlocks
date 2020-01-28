using System.Collections.Generic;

namespace UniBlocks.Schemas.Portal
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public ICollection<AService> Services { get; } = new List<AService>();
    }
}