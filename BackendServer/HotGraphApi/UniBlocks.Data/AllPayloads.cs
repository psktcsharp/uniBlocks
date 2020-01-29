using HotGraphApi.UniBlocks.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data
{
    public class AllPayloads
    {
        public AllPayloads(string anyString)
        {
            AnyString = anyString;
        }
        public string AnyString { get; }
    }
    public class AllServices
    {
        public AllServices(IEnumerable<AService> srvList)
        {
            services = srvList;
        }
        public IEnumerable<AService> services { get; }
    }
}
