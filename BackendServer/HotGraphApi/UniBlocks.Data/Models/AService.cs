using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data.Models
{
    public class AService 
    {
        public AService()
        {
            AServiceSubscriptions = new List<AServiceSubscription>();
        }
        public int AServiceId { get; set; }
        public string ServiceName { get; set; }
        public List<AServiceSubscription> AServiceSubscriptions { get; set; }
    }
}
