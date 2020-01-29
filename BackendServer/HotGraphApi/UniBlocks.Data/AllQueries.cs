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
        public IEnumerable<MsgTest> GetMsgs(
           [Service]IDataRepository dataRepo){

           return dataRepo.AllMsgs;
        }
        public IEnumerable<AService> GetAllServices(
        [Service]IDataRepository dataRepo)
        {

            return dataRepo.AllServices;
        }


    }
}
