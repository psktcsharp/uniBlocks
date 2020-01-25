using System.Collections.Generic;


namespace AspNetCoreGraph.Models
{
    public class Subscription
    {
        public Subscription(){
        this.Services = new HashSet<Service>();        
        }
        public int Id { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
