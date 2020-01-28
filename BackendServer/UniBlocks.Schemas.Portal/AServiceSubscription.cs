using System;
using System.Collections.Generic;
using System.Text;

namespace UniBlocks.Schemas.Portal
{
    class AServiceSubscription
    {
        public int ServiceId { get; set; }
        public AService Service { get; set; }

        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
    }
}
