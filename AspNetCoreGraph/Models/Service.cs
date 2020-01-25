using System.Collections.Generic;

namespace AspNetCoreGraph.Models
{
    public class Service
    {
        public Service(){
        this.Subscriptions = new HashSet<Subscription>();
        }
          public int Id { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}