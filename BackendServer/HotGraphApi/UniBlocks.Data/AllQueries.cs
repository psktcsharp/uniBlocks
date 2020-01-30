using HotChocolate;
using HotChocolate.Types;
using HotGraphApi.UniBlocks.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data
{
    [ExtendObjectType(Name = "Query")]
    public class AllQueries 
    {
        public async Task<List<AService>> ReadServices(
          [Service]UniBlocksDBContext uniBlocks
          )
        {
            return new List<AService>();
        }
    }
}
