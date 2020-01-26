using System.Collections.Generic;

namespace AspNetCoreGraph.Models
{
    public class AService
    {
   
          public int AServiceId { get; set; }
        public string Name { get; set; }
        public ICollection<Subscription> Subscriptions { get; } = new List<Subscription>();
    }
}