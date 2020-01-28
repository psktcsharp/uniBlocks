using System;
using System.Collections.Generic;
using System.Text;

namespace UniBlocks.Schemas.Portal
{
    public class AService
    {
        public int AServiceId { get; set; }
        public string serviceName { get; set; }
        public ICollection<Subscription> Subscriptions { get;  } = new List<Subscription>();
    }
}
