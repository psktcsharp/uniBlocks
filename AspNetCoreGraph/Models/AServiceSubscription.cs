using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreGraph.Models
{
    public class AServiceSubscription
    {
            public int ServiceId { get; set; }
            public AService Service { get; set; }

            public int SubscriptionId { get; set; }
            public Subscription Subscription { get; set; }

    }
}
