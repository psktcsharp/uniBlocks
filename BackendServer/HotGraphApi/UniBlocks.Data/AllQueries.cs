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
        // User Queries
        //--= get all users with messages 
        public List<User> GetUsers(
          [Service]UniBlocksDBContext uniBlocks
          )
        {
            return  uniBlocks.Users
                .Include(user => user.UserMessages).ThenInclude(userMessage => userMessage.Message).ToList();
        }
        //--= get User by id
        public User GetUser(int userId,[Service]UniBlocksDBContext uniBlocks)
        {
            return uniBlocks.Users.Find(userId);
        }
        // Block Queries
        //--= get all blocks 
        public List<Block> GetBlocks(
          [Service]UniBlocksDBContext uniBlocks
          )
        {
            return uniBlocks.Blocks
               .ToList();
        }
        //--= get block by id
        public Block GetBlock(int blockId, [Service]UniBlocksDBContext uniBlocks)
        {
            return uniBlocks.Blocks.Find(blockId);
        }

    }
}
