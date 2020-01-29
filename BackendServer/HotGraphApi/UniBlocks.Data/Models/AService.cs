using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class AService
    {
        public int AServiceId { get; set; }
        public string serviceName { get; set; }
        public ICollection<Subscription> Subscriptions { get; } = new List<Subscription>();
    }
}
