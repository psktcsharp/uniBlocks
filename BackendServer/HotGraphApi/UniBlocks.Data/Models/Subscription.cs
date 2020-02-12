using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class Subscription
    {
        public Subscription()
        {
            AServiceSubscriptions = new List<AServiceSubscription>();
            BlockSubscriptions = new List<BlockSubscriptions>();
          
            User = new User();
        }
        public int SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
     
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AServiceSubscription> AServiceSubscriptions { get; set; }
        public ICollection<BlockSubscriptions> BlockSubscriptions { get; set; }
    }
}
