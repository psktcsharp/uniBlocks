using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class Subscription
    {
        public Subscription()
        {
            AServiceSubscriptions = new List<AServiceSubscription>();
            Balance = new Balance();
        }
        public int SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public Balance Balance { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AServiceSubscription> AServiceSubscriptions { get; set; }
    }
}
