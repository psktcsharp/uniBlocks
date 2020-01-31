using HotChocolate;
using HotChocolate.Types;
using HotGraphApi.UniBlocks.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotGraphApi.UniBlocks.Data
{
    [ExtendObjectType(Name = "Query")]
    public class AllQueries 
    {
        public async Task<List<User>> GetMessages(
          [Service]UniBlocksDBContext uniBlocks
          )
        {

            return uniBlocks.Users
                .Include(user => user.UserMessages).ThenInclude(userMessage => userMessage.Message).ToList();
        }
    }
}
