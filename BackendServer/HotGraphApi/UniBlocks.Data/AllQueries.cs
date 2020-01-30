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
        //public async Task<AllServices> ReadServices(
        //  [Service]UniBlocksDBContext uniBlocks
        //  )
        //{
        //    var result = await uniBlocks.GetServicesAsync();
        //    return new AllServices(result);
        //}
    }
}
